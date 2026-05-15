using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Application.Helpers;
using Domain.Exceptions;

namespace Application.Services;

public class SessaoService(IGeralPersist geralPersist,
                    IAlunoService alunoService,
                    IAlunoGrupoPersist alunoGrupoPersist,
                    IAlunoTurmaPersist alunoTurmaPersist,
                    INotaParcialPersist notaParcialPersist,
                    ISessaoPersist SessaoPersist,
                    ITurmaPersist turmaPersist,
                    IMapper mapper) : ISessaoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IAlunoService _alunoService = alunoService;
    private readonly IAlunoGrupoPersist _alunoGrupoPersist = alunoGrupoPersist;
    private readonly IAlunoTurmaPersist _alunoTurmaPersist = alunoTurmaPersist;
    private readonly INotaParcialPersist _notaParcialPersist = notaParcialPersist;
    private readonly ISessaoPersist _sessaoPersist = SessaoPersist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly IMapper _mapper = mapper;

    #region get
    public async Task<SessaoDTO> GetSessaoById(int Id)
    {
        try
        {
            var Sessao = await _sessaoPersist.GetSessaoIdAsync(Id)
                ?? throw new NotFoundException("Sessão não encontrada");
            return _mapper.Map<SessaoDTO>(Sessao);
        }
        catch
        {
            throw;
        }
    }
    public async Task<SessaoDTO> GetSessaoAtivaTurmaIdAsync(int TurmaId)
    {
        try
        {
            var sessao = await _sessaoPersist.GetSessaoAtivaTurmaIdAsync(TurmaId)
                ?? null!;
            return _mapper.Map<SessaoDTO>(sessao);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<SessaoDTO>> GetSessoesTurmaIdAsync(int turmaId)
    {
        try
        {
            var sessoes = await _sessaoPersist.GetSessoesTurmaIdAsync(turmaId)
                ?? null!;
            return _mapper.Map<IEnumerable<SessaoDTO>>(sessoes);
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<AvaliacaoConsolidadaExportDTO>> GetAvaliacaoConsolidada(int sessaoId)
    {
        try
        {
            var sessao = await _sessaoPersist.GetSessaoIdAsync(sessaoId) 
                ?? throw new NotFoundException("Sessão inválida");
            var notasParciais = await _notaParcialPersist.GetNotaParcialSessaoIdAsync(sessaoId);
            var alunos = await _alunoService.GetAlunosTurma(sessao.TurmaId);

            var alunoNota = alunos
                .Select(a => {
                    var notas = notasParciais.Where(np => np.AvaliadoId == a.Id);
                    return new AvaliacaoConsolidadaExportDTO
                    {
                        Aluno = a.Nome ??
                            "Não identificado",
                        Nota = notas.Any()
                            ? notas.Average(c => c.Nota)
                            : 0
                    };
                })
                .OrderBy(an => an.Aluno)
                .ToList();

            return alunoNota; 
        }
        catch
        {
            throw;
        }
    }
    public async Task<SessaoValidacaoDTO> GetValidaInicioSessao(int turmaId)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Turma não encontrada");

            var retorno = new SessaoValidacaoDTO();

            var alunos = turma.Alunos;
            var alunoGrupos = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(turmaId);
            var alunoComGrupoIds = alunoGrupos.Select(ag => ag.AlunoId).ToHashSet();
            var alunosSemGrupo = alunos.Where(a => !alunoComGrupoIds.Contains(a.Id));

            if(alunos.Count == 0)
            {
                retorno.Mensagens.Add(new SessaoValidacaoMensagemDTO
                {
                   Tipo = "Erro",
                   Mensagem = "Nenhum aluno vinculado à turma."
                });

                retorno.PodeIniciar = false;
            }

            if(alunoGrupos.Length == 0)
            {                
                retorno.Mensagens.Add(new SessaoValidacaoMensagemDTO
                {
                   Tipo = "Erro",
                   Mensagem = "Sem vínculos de alunos à equipe na turma."
                });

                retorno.PodeIniciar = false;
            }

            if(turma.Criterios.Count == 0)
            {                
                retorno.Mensagens.Add(new SessaoValidacaoMensagemDTO
                {
                   Tipo = "Erro",
                   Mensagem = "Nenhum critério vinculado à turma."
                });

                retorno.PodeIniciar = false;
            }

            if(alunosSemGrupo.Any())
            {                
                retorno.Mensagens.Add(new SessaoValidacaoMensagemDTO
                {
                   Tipo = "Aviso",
                   Mensagem = "Existem alunos sem equipe."
                });
            }

            return retorno;
        } catch {
            throw;
        }
    }
    public async Task<IEnumerable<AlunoDTO>> GetFaltamAvaliarSessao(int sessaoId)
    {
        try
        {
            var sessao = await _sessaoPersist.GetSessaoIdAsync(sessaoId)
                ?? throw new NotFoundException("Sessão não encontrada");

            var turma = await _turmaPersist.GetTurmaIdAsync(sessao.TurmaId);
            var alunos = turma!.Alunos;
            var notasFinais = sessao.Notasfinais;

            var alunosAvaliaram = notasFinais.Select(n => n.AvaliadorId).ToHashSet();

            var alunosSemAvaliar = alunos.Where(a => !alunosAvaliaram.Contains(a.Id));

            return _mapper.Map<IEnumerable<AlunoDTO>>(alunosSemAvaliar);
        } catch {
            throw;
        }
    }
    #endregion
    #region add
    public async Task<SessaoDTO> Add(SessaoDTO model)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(model.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            if(await _sessaoPersist.GetSessaoAtivaTurmaIdAsync(model.TurmaId) != null)
                throw new BusinessException("Já existe Avaliação ativa para esta turma.");
            var sessao = new Sessao
            (
                turma: turma,
                dataInicio: DateTime.UtcNow,
                tokenPublico: PublicToken.GeneratePublicToken()
            );
            _geralPersist.Add(sessao);
            _ = await _geralPersist.SaveChangesAsync();
            var SessaoRetorno = await _sessaoPersist.GetSessaoIdAsync(sessao.Id);
            return _mapper.Map<SessaoDTO>(SessaoRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update
    public async Task<SessaoDTO> EncerrarSessao(int SessaoId, SessaoDTO model)
    {
        try
        {
            var Sessao = await _sessaoPersist.GetSessaoIdAsync(SessaoId)
                ?? throw new NotFoundException("Sessão não encontrada");

            Sessao.EncerrarSessao(DateTime.UtcNow);
            _ = await _geralPersist.SaveChangesAsync();

            var SessaoRetorno = await _sessaoPersist.GetSessaoIdAsync(SessaoId);
            return _mapper.Map<SessaoDTO>(SessaoRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
}
