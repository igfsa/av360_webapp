using StackExchange.Redis;

using Application.Contracts;
using Application.DTOs;
using Persistence.Contracts;
using Domain.Exceptions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;
public class DashboardSessaoService(
                                        IAlunoTurmaPersist alunoTurmaPersist, 
                                        INotaFinalPersist notaFinalPersist,
                                        INotaParcialPersist notaParcialPersist,
                                        ICriterioTurmaPersist criterioturmaPersist,
                                        IGrupoPersist grupoPersist,
                                        ITurmaPersist turmaPersist,
                                        IResultadoPersist resultadoPersist,
                                        IAlunoGrupoPersist alunoGrupoPersist,
                                        ISessaoPersist sessaoPersist, 
                                        IDashboardCacheService cache,
                                        ILogger<DashboardSessaoService> logger
                                    ) : IDashboardSessaoService
{
    private readonly IDashboardCacheService _cache = cache;
    private readonly IAlunoTurmaPersist _alunoTurmaPersist = alunoTurmaPersist;
    private readonly INotaFinalPersist _notaFinalPersist = notaFinalPersist;
    private readonly INotaParcialPersist _notaParcialPersist = notaParcialPersist;
    private readonly ICriterioTurmaPersist _criterioTurmaPersist = criterioturmaPersist;
    private readonly IGrupoPersist _grupoPersist = grupoPersist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly IResultadoPersist _resultadoPersist = resultadoPersist;
    private readonly IAlunoGrupoPersist _alunoGrupoPersist = alunoGrupoPersist;
    private readonly ISessaoPersist _sessaoPersist = sessaoPersist;
    private readonly ILogger _logger = logger;

    private async Task<DashboardSessaoDTO> BuildFromDatabase(int sessaoId)
    {
        var sessao = await _sessaoPersist.GetSessaoIdAsync(sessaoId)
                ?? throw new NotFoundException("Sessão não encontrada");

        var turma = await _turmaPersist.GetTurmaIdAsync(sessao.TurmaId)
                ?? throw new NotFoundException("Sessão não encontrada");

        var notasFinais = await _notaFinalPersist.GetNotasFinalSessaoIdAsync(sessaoId);

        var notasSessao = await _notaParcialPersist.GetNotaParcialSessaoIdAsync(sessaoId);

        var alunos = await _alunoTurmaPersist.GetAlunosTurmaIdAsync(sessao.TurmaId);

        var criteriosTurma = await _criterioTurmaPersist.GetCriteriosTurmaIdAsync(sessao.TurmaId);

        var gruposTurma = await _grupoPersist.GetGruposTurmaIdAsync(sessao.TurmaId);

        var alunosGrupo = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(sessao.TurmaId);

        var notasPorAluno = notasSessao
            .GroupBy(n => n.AvaliadoId)
            .ToDictionary(g => g.Key, g => g.ToList());
        
        var notasPorCriterio = notasSessao
            .GroupBy(n => n.CriterioId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var alunosPorGrupo = alunosGrupo
            .GroupBy(ag => ag.GrupoId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.AlunoId).ToHashSet());

        var alunosDto = alunos.Select(a => 
        {
            var notasAluno = notasPorAluno.GetValueOrDefault(a.Id, []);
            var totalNotas = notasAluno.Count;

            var mediasCriterios = new List<CriterioAlunoDashboardDTO>();

            foreach (var c in criteriosTurma)
            {
                var criterioNotas = notasAluno.Where(n => n.CriterioId == c.Id).ToArray();
                var totalCriterio = criterioNotas.Length;
                
                mediasCriterios.Add(new CriterioAlunoDashboardDTO
                {
                    CriterioId = c.Id,
                    Nome = c.Nome,
                    Media = totalCriterio > 0 ? criterioNotas.Average(n => n.Nota) : 0,
                    TotalNotas = totalCriterio
                });
            }

            return new AlunoDashboardDTO
            {
                AlunoId = a.Id,
                Nome = a.Nome,
                TotalNotas = totalNotas,
                Media = totalNotas > 0 ? notasAluno.Average(n => n.Nota) : 0,
                GrupoId = alunosGrupo.FirstOrDefault(ag => ag.AlunoId == a.Id)?.GrupoId,
                Avaliou = notasFinais.Any(nf => nf.AvaliadorId == a.Id),
                CriterioAluno = mediasCriterios,
                Comentario = notasFinais.FirstOrDefault(nf => nf.AvaliadorId == a.Id)?.ComentarioAluno
            };
        }).ToList();

        var criteriosDto = criteriosTurma.Select(c =>
        {
            var notasCriterio = notasPorCriterio.GetValueOrDefault(c.Id, []);
            var totalNotasCriterio = notasCriterio.Count; 

            return new CriterioDashboardDTO
            {
                CriterioId = c.Id,
                Nome = c.Nome,
                MediaGlobal = totalNotasCriterio > 0 ? notasCriterio.Average(n => n.Nota) : 0,
                TotalNotas = totalNotasCriterio
            };
        }).ToList();

        var gruposDto = gruposTurma.Select(g =>
        {
            var notasAluno = alunosPorGrupo.GetValueOrDefault(g.Id, []);
            var notasGrupo = notasSessao
                .Where(ng => notasAluno
                    .Contains(ng.AvaliadoId)).ToList();
            var totalnotasGrupo = notasGrupo.Count;
            var alunosGrupo = alunosDto.Where(a => a.GrupoId == g.Id).ToList();

            return new GrupoDashboardDTO
            {
                GrupoId = g.Id,
                Nome = g.Nome,
                Media = totalnotasGrupo > 0 ? notasGrupo.Average(n => n.Nota) : 0,
                TotalNotas = totalnotasGrupo,
                Avaliaram = alunosGrupo.Count(a => a.Avaliou),
                Pendentes = alunosGrupo.Count - alunosGrupo.Count(a => a.Avaliou),
                Alunos = alunosGrupo
            };
        }).ToList();

        var alunosSemGrupo = alunosDto
            .Where(a => !a.GrupoId.HasValue || a.GrupoId == 0)
            .ToList();

        if (alunosSemGrupo.Count > 0)
        {
            gruposDto.Add(new GrupoDashboardDTO
            {
                GrupoId = 0,
                Nome = "Sem Equipe",
                Media = 0,
                TotalNotas = 0,
                Avaliaram = 0,
                Pendentes = alunosSemGrupo.Count,
                Alunos = alunosSemGrupo
            });
        }

        var avaliaram = alunosDto.Count(a => a.Avaliou);

        var mediaGeral = notasSessao.Length > 0 ? notasSessao.Average(n => n.Nota) : 0;
        var totalNotas = notasSessao.Length;

        return new DashboardSessaoDTO
        {
            SessaoId = sessaoId,
            TotalAlunos = alunos.Length,
            Avaliaram = avaliaram,
            Pendentes = alunos.Length - avaliaram,
            MediaGeral = mediaGeral,
            TotalNotas = totalNotas,
            NotaMax = turma.NotaMax,
            TurmaCod = turma.Cod,
            Criterios = criteriosDto,
            Grupos = gruposDto
        };
    }
    public async Task<DashboardSessaoDTO?> GetDashboard(int sessaoId)
    {
        try
        {
            var cached = await _cache.GetAsync(sessaoId);
            if (cached != null)
                return cached;

            var dto = await BuildFromDatabase(sessaoId);

            await _cache.SetAsync(sessaoId, dto);

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar Dashboard: {sessaoId}", sessaoId);
            throw;
        }
    }
    public async Task<DashboardSessaoDTO?> ResetDashboard(int sessaoId)
    {
        await _cache.RemoveAsync(sessaoId);

        var dto = await BuildFromDatabase(sessaoId);

        await _cache.SetAsync(sessaoId, dto);

        return dto;
    }

    public async Task<DashboardSessaoDTO> GetResultadoDashboard(int sessaoId)
    {
        try {
            var sessaoResultado = await _resultadoPersist.GetResultadoSessaoIdAsync(sessaoId)
                    ?? throw new NotFoundException("Sessão não encontrada");

            var notasFinais = await _resultadoPersist.GetNotasFinalResultadoSessaoIdAsync(sessaoResultado.Id);

            var notasSessao = await _resultadoPersist.GetNotaParcialResultadoSessaoIdAsync(sessaoResultado.Id);

            var alunos = await _resultadoPersist.GetAlunosResultadoSessaoIdAsync(sessaoResultado.Id);

            var criterios =  await _resultadoPersist.GetCriteriosResultadoSessaoIdAsync(sessaoResultado.Id);

            var grupos = await _resultadoPersist.GetGruposResultadoSessaoIdAsync(sessaoResultado.Id);

            var notasPorAluno = notasSessao
                .GroupBy(n => n.AvaliadoResId)
                .ToDictionary(g => g.Key, g => g.ToList());
            
            var notasPorCriterio = notasSessao
                .GroupBy(n => n.CriterioResId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var notasPorGrupo = notasFinais
                .GroupBy(ag => ag.GrupoResId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var alunosPorGrupo = alunos
                .GroupBy(a => a.ResultadoGrupoId ?? 0)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Id).ToHashSet());

            var alunosDto = alunos.Select(a => 
            {
                var notasAluno = notasPorAluno.GetValueOrDefault(a.Id, []);
                var totalNotas = notasAluno.Count;

                var mediasCriterios = new List<CriterioAlunoDashboardDTO>();

                foreach (var c in criterios)
                {
                    var criterioNotas = notasAluno.Where(n => n.CriterioResId == c.Id).ToArray();
                    var totalCriterio = criterioNotas.Length;
                    
                    mediasCriterios.Add(new CriterioAlunoDashboardDTO
                    {
                        CriterioId = c.Id,
                        Nome = c.Nome,
                        Media = totalCriterio > 0 ? criterioNotas.Average(n => n.Nota) : 0,
                        TotalNotas = totalCriterio
                    });
                }

                return new AlunoDashboardDTO
                {
                    AlunoId = a.Id,
                    Nome = a.Nome,
                    TotalNotas = totalNotas,
                    Media = totalNotas > 0 ? notasAluno.Average(n => n.Nota) : 0,
                    GrupoId = a.ResultadoGrupoId,
                    Avaliou = notasFinais.Any(nf => nf.AvaliadorResId == a.Id),
                    CriterioAluno = mediasCriterios,
                    Comentario = notasFinais.FirstOrDefault(nf => nf.AvaliadorResId == a.Id)?.ComentarioAluno
                };
            }).ToList();

            var criteriosDto = criterios.Select(c =>
            {
                var notasCriterio = notasPorCriterio.GetValueOrDefault(c.Id, []);
                var totalNotasCriterio = notasCriterio.Count; 

                return new CriterioDashboardDTO
                {
                    CriterioId = c.Id,
                    Nome = c.Nome,
                    MediaGlobal = totalNotasCriterio > 0 ? notasCriterio.Average(n => n.Nota) : 0,
                    TotalNotas = totalNotasCriterio
                };
            }).ToList();

            var gruposDto = grupos.Select(g =>
            {
                var notasAluno = alunosPorGrupo.GetValueOrDefault(g.Id, []);
                var notasGrupo = notasSessao
                    .Where(ng => notasAluno
                        .Contains(ng.AvaliadoResId)).ToList();
                var totalnotasGrupo = notasGrupo.Count;
                var alunosGrupo = alunosDto.Where(a => a.GrupoId == g.Id).ToList();

                return new GrupoDashboardDTO
                {
                    GrupoId = g.Id,
                    Nome = g.Nome,
                    Media = totalnotasGrupo > 0 ? notasGrupo.Average(n => n.Nota) : 0,
                    TotalNotas = totalnotasGrupo,
                    Avaliaram = alunosGrupo.Count(a => a.Avaliou),
                    Pendentes = alunosGrupo.Count - alunosGrupo.Count(a => a.Avaliou),
                    Alunos = alunosGrupo
                };
            }).ToList();

            var alunosSemGrupo = alunosDto
                .Where(a => !a.GrupoId.HasValue)
                .ToList();

            if (alunosSemGrupo.Count > 0)
            {
                gruposDto.Add(new GrupoDashboardDTO
                {
                    GrupoId = 0,
                    Nome = "Sem Equipe",
                    Media = 0,
                    TotalNotas = 0,
                    Avaliaram = 0,
                    Pendentes = alunosSemGrupo.Count,
                    Alunos = alunosSemGrupo
                });
            }

            var avaliaram = alunosDto.Count(a => a.Avaliou);

            var mediaGeral = notasSessao.Length > 0 ? notasSessao.Average(n => n.Nota) : 0;
            var totalNotas = notasSessao.Length;

            return new DashboardSessaoDTO
            {
                SessaoId = sessaoId,
                TotalAlunos = alunos.Length,
                Avaliaram = avaliaram,
                Pendentes = alunos.Length - avaliaram,
                MediaGeral = mediaGeral,
                TotalNotas = totalNotas,
                NotaMax = sessaoResultado.NotaMaxima,
                TurmaCod = sessaoResultado.TurmaCod,
                Criterios = criteriosDto,
                Grupos = gruposDto,
                Inconsistencia = sessaoResultado.Inconsistencia 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar Dashboard: {sessaoId}", sessaoId);
            throw;
        }
    }
}