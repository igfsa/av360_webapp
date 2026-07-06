using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Application.Helpers;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ResultadoService(IGeralPersist geralPersist,
                    IAlunoGrupoPersist alunoGrupoPersist,
                    INotaParcialPersist notaParcialPersist,
                    ISessaoPersist SessaoPersist,
                    IResultadoPersist ResultadoPersist,
                    ICriterioTurmaPersist criterioTurmaPersist,
                    IAlunoTurmaPersist alunoTurmaPerist,
                    ITurmaPersist turmaPersist,
                    IGrupoPersist grupoPersist,
                    ILogger<ResultadoService> logger) : IResultadoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IAlunoGrupoPersist _alunoGrupoPersist = alunoGrupoPersist;
    private readonly INotaParcialPersist _notaParcialPersist = notaParcialPersist;
    private readonly ISessaoPersist _sessaoPersist = SessaoPersist;
    private readonly IResultadoPersist _resultadoPersist = ResultadoPersist;
    private readonly ICriterioTurmaPersist _criterioturmaPersist = criterioTurmaPersist;
    private readonly IAlunoTurmaPersist _alunoTurmaPerist = alunoTurmaPerist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly IGrupoPersist _grupoPersist = grupoPersist;
    private readonly ILogger _logger = logger;

    #region get

    #endregion
    #region add

    #endregion
    #region update
    public async Task<ResultadoSessaoReportDTO> RecalcularResultadoSessao(int sessaoId)
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
            var resultadoReport = new ResultadoSessaoReportDTO();

            var sessaoResult = await _resultadoPersist.GetResultadoSessaoIdAsync(sessaoId);

            if (sessaoResult == null)
            {
                sessaoResult  = new ResultadoSessao(sessao, turma);
                _geralPersist.Add(sessaoResult);
                _ = await _geralPersist.SaveChangesAsync();
            }
            
            var alunosResultado = sessaoResult.Alunos
                .ToDictionary(a => a.AlunoId);

            var gruposResultado = sessaoResult.Grupos
                .ToDictionary(g => g.GrupoId);

            var criteriosResultado = sessaoResult.Criterios
                .ToDictionary(c => c.CriterioId);

            var notasFinaisResultado = sessaoResult.NotasFinais
                .ToDictionary(n => n.NotaFinalId);

            foreach (var c in criteriosTurma)
            {
                resultadoReport.TotalGeral++;
                resultadoReport.TotalCriterios++;

                if (!criteriosResultado.ContainsKey(c.Id))
                {
                    try
                    {
                        sessaoResult.AdicionarCriterio(c);
                        _ = await _geralPersist.SaveChangesAsync();
                    } catch (Exception ex) {
                        resultadoReport.ErrosCriterios.Add(new ResultadoSessaoErrorDTO
                        {
                            Tipo = "criterio",
                            Nome = c.Nome,
                            Erro = InnerErrorMessage.ObterMensagemErro(ex)
                        });
                    }
                }
            }

            criteriosResultado = sessaoResult.Criterios
                .ToDictionary(c => c.CriterioId);

            foreach (var g in gruposTurma)
            {
                resultadoReport.TotalGeral++;
                resultadoReport.TotalGrupos++;

                if (!gruposResultado.ContainsKey(g.Id))
                {
                    try
                    {
                        sessaoResult.AdicionarGrupo(g); 
                        _ = await _geralPersist.SaveChangesAsync();
                    } catch (Exception ex) {
                        resultadoReport.ErrosGrupos.Add(new ResultadoSessaoErrorDTO
                        {
                            Tipo = "grupo",
                            Nome = g.Nome,
                            Erro = InnerErrorMessage.ObterMensagemErro(ex)
                        });
                    }
                }
            }

            gruposResultado = sessaoResult.Grupos
                .ToDictionary(g => g.GrupoId);

            var grupoAluno = alunosGrupo.ToDictionary(
                x => x.AlunoId,
                x => (int?)x.GrupoId);

            foreach (var a in alunos)
            {
                resultadoReport.TotalGeral++;
                resultadoReport.TotalAlunos++;

                if (!alunosResultado.ContainsKey(a.Id))
                {
                    try
                    {
                        grupoAluno.TryGetValue(a.Id, out var grupoId);

                        ResultadoGrupo? grupo = null;

                        if (grupoId.HasValue)
                            gruposResultado.TryGetValue(grupoId.Value, out grupo);

                        sessaoResult.AdicionarAluno(a, grupo);
                        _ = await _geralPersist.SaveChangesAsync();
                    } catch (Exception ex) {
                        resultadoReport.ErrosAlunos.Add(new ResultadoSessaoErrorDTO
                        {
                            Tipo = "aluno",
                            Nome = a.Nome,
                            Erro = InnerErrorMessage.ObterMensagemErro(ex)
                        });
                    }
                }
            }

            alunosResultado = sessaoResult.Alunos
                .ToDictionary(a => a.AlunoId);

            var notasParciaisResultado =
                await _resultadoPersist.GetNotaParcialResultadoSessaoIdAsync(sessaoResult.Id);

            var indiceNotasParciais = notasParciaisResultado.ToDictionary(x => x.NotaParcialId);

            foreach(var nf in notasFinais)
            {
                resultadoReport.TotalGeral++;
                resultadoReport.TotalNotasFinais++;
                var avaliadorRes = alunosResultado[nf.AvaliadorId]
                    ?? throw new NotFoundException("Aluno Avaliador não identificado");
                try
                {
                    var grupoRes = gruposResultado[nf.GrupoId]
                        ?? throw new NotFoundException("Grupo não identificado");
                    
                    if (!notasFinaisResultado.TryGetValue(nf.Id, out var nFinalRes))
                    {
                        nFinalRes = new ResultadoNotaFinal(sessaoResult, nf, avaliadorRes, grupoRes);
                        _geralPersist.Add(nFinalRes);
                        _ = await _geralPersist.SaveChangesAsync();
                    }

                    foreach(var np in nf.NotasParcial)
                    {
                        resultadoReport.TotalGeral++;
                        resultadoReport.TotalNotasParciais++;
                        var avaliadoRes = alunosResultado[np.AvaliadoId]
                            ?? throw new NotFoundException("Aluno Avaliado não identificado");
                        
                        if (!indiceNotasParciais.ContainsKey(np.Id))
                        {
                            try {
                                var criterioRes = criteriosResultado[np.CriterioId]
                                    ?? throw new NotFoundException("Critério não identificado");

                                var nParcialRes = new ResultadoNotaParcial(nFinalRes, np, avaliadoRes, criterioRes);
                                _geralPersist.Add(nParcialRes);
                                _ = await _geralPersist.SaveChangesAsync();
                            } catch (Exception ex) {
                                resultadoReport.ErrosNotasParciais.Add(new ResultadoSessaoErrorDTO
                                {
                                    Tipo = "notaParcial",
                                    Nome = avaliadorRes.Nome,
                                    Erro = InnerErrorMessage.ObterMensagemErro(ex)
                                });
                            }
                        }
                    }
                } catch (Exception ex) {
                    resultadoReport.ErrosNotasFinais.Add(new ResultadoSessaoErrorDTO
                    {
                        Tipo = "notaFinal",
                        Nome = avaliadorRes.Nome,
                        Erro = InnerErrorMessage.ObterMensagemErro(ex)
                    });
                }
            }

            sessaoResult.EditInconsistencia(resultadoReport.TotalGeralErros != 0);
            _ = await _geralPersist.SaveChangesAsync();

            return resultadoReport;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar encerrar sessaoId: {sessaoId}", sessaoId);
            throw;
        }

    }
    #endregion
}
