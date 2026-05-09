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
                    INotaParcialPersist notaParcialPersist,
                    ISessaoPersist SessaoPersist,
                    ITurmaPersist turmaPersist,
                    IMapper mapper) : ISessaoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IAlunoService _alunoService = alunoService;
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
            var alunosDict = alunos.ToDictionary(a => a.Id, a => a.Nome);

            var alunoNota = notasParciais
                .GroupBy(n => n.AvaliadoId)
                .Select(np => new AvaliacaoConsolidadaExportDTO
                {
                    Aluno = alunosDict.GetValueOrDefault(
                        np.Key,
                        "Não identificado"
                    ),
                    Nota = np.Average(c => c.Nota)
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
    #endregion
    #region add
    public async Task<SessaoDTO> Add(SessaoDTO model)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(model.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
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
