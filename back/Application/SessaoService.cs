using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Application.Helpers;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class SessaoService(IGeralPersist geralPersist,
                    IAlunoService alunoService,
                    IAlunoGrupoPersist alunoGrupoPersist,
                    INotaParcialPersist notaParcialPersist,
                    INotaFinalPersist notaFinalPersist,
                    ISessaoPersist SessaoPersist,
                    IResultadoPersist ResultadoPersist,
                    ICriterioTurmaPersist criterioTurmaPersist,
                    IAlunoTurmaPersist alunoTurmaPerist,
                    ITurmaPersist turmaPersist,
                    IGrupoPersist grupoPersist,
                    IMapper mapper,
                    ILogger<SessaoService> logger) : ISessaoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IAlunoService _alunoService = alunoService;
    private readonly IAlunoGrupoPersist _alunoGrupoPersist = alunoGrupoPersist;
    private readonly INotaParcialPersist _notaParcialPersist = notaParcialPersist;
    private readonly INotaFinalPersist _notaFinalPersist = notaFinalPersist;
    private readonly ISessaoPersist _sessaoPersist = SessaoPersist;
    private readonly IResultadoPersist _resultadoPersist = ResultadoPersist;
    private readonly ICriterioTurmaPersist _criterioturmaPersist = criterioTurmaPersist;
    private readonly IAlunoTurmaPersist _alunoTurmaPerist = alunoTurmaPerist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly IGrupoPersist _grupoPersist = grupoPersist;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

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

            List<AvaliacaoConsolidadaExportDTO> alunoNota;

            if (sessao.Ativo)
            {
                var notasParciais = await _notaParcialPersist.GetNotaParcialSessaoIdAsync(sessaoId);
                var notasFinais = await _notaFinalPersist.GetNotasFinalSessaoIdAsync(sessaoId);
                var alunos = await _alunoService.GetAlunosTurma(sessao.TurmaId);

                alunoNota = [.. alunos
                    .Select(a => {
                        var notas = notasParciais.Where(np => np.AvaliadoId == a.Id);
                        return new AvaliacaoConsolidadaExportDTO
                        {
                            Aluno = a.Nome ??
                                "Não identificado",
                            Nota = notas.Any()
                                ? notas.Average(c => c.Nota)
                                : 0,
                            Avaliou = notasFinais.Any(nf => nf.AvaliadorId == a.Id)
                        };
                    })
                    .OrderBy(an => an.Aluno)];
            } else {
                var sessaoRes = await _resultadoPersist.GetResultadoSessaoIdAsync(sessaoId)
                    ?? throw new NotFoundException("Resultado Sessão não encontrado");
                var notasParciais = await _resultadoPersist.GetNotaParcialResultadoSessaoIdAsync(sessaoRes.Id);
                var notasFinais = await _notaFinalPersist.GetNotasFinalSessaoIdAsync(sessaoId);
                var alunos = await _resultadoPersist.GetAlunosResultadoSessaoIdAsync(sessaoRes.Id);

                alunoNota = [.. alunos
                    .Select(a => {
                        var notas = notasParciais.Where(np => np.AvaliadoResId == a.Id);
                        return new AvaliacaoConsolidadaExportDTO
                        {
                            Aluno = a.Nome ??
                                "Não identificado",
                            Nota = notas.Any()
                                ? notas.Average(c => c.Nota)
                                : 0,
                            Avaliou = notasFinais.Any(nf => nf.AvaliadorId == a.Id)
                        };
                    })
                    .OrderBy(an => an.Aluno)];
            }

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

            var sessao = new Sessao(turma, DateTime.UtcNow, PublicToken.GeneratePublicToken());
            _geralPersist.Add(sessao);
            _ = await _geralPersist.SaveChangesAsync();
            
            var SessaoRetorno = await _sessaoPersist.GetSessaoIdAsync(sessao.Id);
            return _mapper.Map<SessaoDTO>(SessaoRetorno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar sessão turmaId: {turmaId}", model.TurmaId);
            throw;
        }
    }
    #endregion
    #region update
    public async Task<ResultadoSessaoReportDTO> EncerrarSessao(int sessaoId)
    {
        try
        {
            var sessao = await _sessaoPersist.GetSessaoIdAsync(sessaoId)
                ?? throw new NotFoundException("Sessão não encontrada");
            var turma = await _turmaPersist.GetTurmaIdAsync(sessao.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            var alunos = await _alunoTurmaPerist.GetAlunosTurmaIdAsync(turma.Id);
            var notasFinais = sessao.Notasfinais;
            var notasSessao = await _notaParcialPersist.GetNotaParcialSessaoIdAsync(sessaoId);
            var criteriosTurma = await _criterioturmaPersist.GetCriteriosTurmaIdAsync(turma.Id);
            var gruposTurma = await _grupoPersist.GetGruposTurmaIdAsync(sessao.TurmaId);
            var alunosGrupo = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(sessao.TurmaId);
            var resultado = new ResultadoSessaoReportDTO();

            sessao.EncerrarSessao(DateTime.UtcNow);
            _ = await _geralPersist.SaveChangesAsync();

            var sessaoResult = new ResultadoSessao(sessao, turma);
            _geralPersist.Add(sessaoResult);
            _ = await _geralPersist.SaveChangesAsync();

            foreach (var c in criteriosTurma)
            {
                resultado.TotalGeral++;
                resultado.TotalCriterios++;
                try
                {
                    sessaoResult.AdicionarCriterio(c);
                    _ = await _geralPersist.SaveChangesAsync();
                } catch (Exception ex) {
                    resultado.ErrosCriterios.Add(new ResultadoSessaoErrorDTO
                    {
                        Tipo = "criterio",
                        Nome = c.Nome,
                        Erro = InnerErrorMessage.ObterMensagemErro(ex)
                    });
                }
            }

            foreach (var g in gruposTurma)
            {
                resultado.TotalGeral++;
                resultado.TotalGrupos++;
                try
                {
                    sessaoResult.AdicionarGrupo(g); 
                    _ = await _geralPersist.SaveChangesAsync();
                } catch (Exception ex) {
                    resultado.ErrosGrupos.Add(new ResultadoSessaoErrorDTO
                    {
                        Tipo = "grupo",
                        Nome = g.Nome,
                        Erro = InnerErrorMessage.ObterMensagemErro(ex)
                    });
                }
            }

            foreach (var a in alunos)
            {
                resultado.TotalGeral++;
                resultado.TotalAlunos++;
                try
                {
                    var grupoId = alunosGrupo.FirstOrDefault(ag => ag.AlunoId == a.Id)?.GrupoId ?? 0;
                    var grupo = sessaoResult.Grupos.FirstOrDefault(g=> g.GrupoId == grupoId);
                    sessaoResult.AdicionarAluno(a, grupo);
                    _ = await _geralPersist.SaveChangesAsync();
                } catch (Exception ex) {
                    resultado.ErrosAlunos.Add(new ResultadoSessaoErrorDTO
                    {
                        Tipo = "aluno",
                        Nome = a.Nome,
                        Erro = InnerErrorMessage.ObterMensagemErro(ex)
                    });
                }
            }

            foreach(var nf in notasFinais)
            {
                resultado.TotalGeral++;
                resultado.TotalNotasFinais++;
                var avaliadorRes = sessaoResult.Alunos.FirstOrDefault(a => a.AlunoId == nf.AvaliadorId)
                    ?? throw new NotFoundException("Aluno não identificado");
                try
                {
                    var grupoRes = sessaoResult.Grupos.FirstOrDefault(g => g.GrupoId == nf.GrupoId)
                        ?? throw new NotFoundException("Grupo não identificado");
                    var nFinalRes = new ResultadoNotaFinal(sessaoResult, nf, avaliadorRes, grupoRes);
                    _geralPersist.Add(nFinalRes);
                    _ = await _geralPersist.SaveChangesAsync();
                    foreach(var np in nf.NotasParcial)
                    {
                        resultado.TotalGeral++;
                        resultado.TotalNotasParciais++;
                        var avaliadoRes = sessaoResult.Alunos.FirstOrDefault(a => a.AlunoId == np.AvaliadoId)
                            ?? throw new NotFoundException("Aluno não identificado");
                        try {
                            var criterioRes = sessaoResult.Criterios.FirstOrDefault(c => c.CriterioId == np.CriterioId)
                                ?? throw new NotFoundException("Aluno não identificado");

                            var nParcialRes = new ResultadoNotaParcial(nFinalRes, np, avaliadoRes, criterioRes);
                            _geralPersist.Add(nParcialRes);
                            _ = await _geralPersist.SaveChangesAsync();
                        } catch (Exception ex) {
                            resultado.ErrosNotasParciais.Add(new ResultadoSessaoErrorDTO
                            {
                                Tipo = "notaParcial",
                                Nome = avaliadorRes.Nome,
                                Erro = InnerErrorMessage.ObterMensagemErro(ex)
                            });
                        }
                    }
                } catch (Exception ex) {
                    resultado.ErrosNotasFinais.Add(new ResultadoSessaoErrorDTO
                    {
                        Tipo = "notaFinal",
                        Nome = avaliadorRes.Nome,
                        Erro = InnerErrorMessage.ObterMensagemErro(ex)
                    });
                }
            }

            sessaoResult.EditInconsistencia(resultado.TotalGeralErros != 0);
            _ = await _geralPersist.SaveChangesAsync();

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar encerrar sessaoId: {sessaoId}", sessaoId);
            throw;
        }

    }
    #endregion
}
