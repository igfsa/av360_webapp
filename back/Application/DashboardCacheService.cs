using System.Text.Json;
using StackExchange.Redis;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;

namespace Application.Services;

public class DashboardCacheService(IConnectionMultiplexer connection) : IDashboardCacheService
{
    private readonly IDatabase _redis = connection.GetDatabase();

    private static string Key(int sessaoId) => $"sessao:{sessaoId}:dashboard";

    public async Task<DashboardSessaoDTO?> GetAsync(int sessaoId)
    {
        var data = await _redis.StringGetAsync(Key(sessaoId));

        if (data.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<DashboardSessaoDTO>((string)data!);
    }

    public async Task SetAsync(int sessaoId, DashboardSessaoDTO dto)
    {
        var json = JsonSerializer.Serialize(dto);

        await _redis.StringSetAsync(
            Key(sessaoId),
            json,
            TimeSpan.FromMinutes(30) 
        );
    }

    public async Task RemoveAsync(int sessaoId)
    {
        await _redis.KeyDeleteAsync(Key(sessaoId));
    }

    public async Task AtualizarNotaAsync(
        int sessaoId,
        int alunoId,
        int criterioId,
        int grupoId,
        decimal nota)
    {

        var dto = await GetAsync(sessaoId);

        if (dto == null)
            return; 
        
        var grupo = dto.Grupos.First(g => g.GrupoId == grupoId);
        var aluno = grupo.Alunos.First(a => a.AlunoId == alunoId);

        aluno.Media = AtualizarMedia(aluno.Media, aluno.TotalNotas, nota);
        aluno.TotalNotas++;

        var criterioAluno = aluno.CriterioAluno
            .First(c => c.CriterioId == criterioId);

        criterioAluno.Media = AtualizarMedia(
            criterioAluno.Media,
            criterioAluno.TotalNotas,
            nota);

        criterioAluno.TotalNotas++;

        var criterioGlobal = dto.Criterios
            .First(c => c.CriterioId == criterioId);

        criterioGlobal.MediaGlobal = AtualizarMedia(
            criterioGlobal.MediaGlobal,
            criterioGlobal.TotalNotas,
            nota);

        criterioGlobal.TotalNotas++;

        grupo.Media = AtualizarMedia(
            grupo.Media,
            grupo.TotalNotas,
            nota);

        grupo.TotalNotas++;

        dto.MediaGeral = AtualizarMedia(
            dto.MediaGeral,
            dto.TotalNotas,
            aluno.Media
        );

        await SetAsync(sessaoId, dto);
    }

    public async Task AtualizarAlunoAsync(int sessaoId, int alunoId, int grupoId)
    {
        var dto = await GetAsync(sessaoId);
        if (dto == null)
            return; 

        var grupo = dto.Grupos.FirstOrDefault(g => g.GrupoId == grupoId);
        if (grupo == null)
            return;

        var aluno = grupo.Alunos.FirstOrDefault(a => a.AlunoId == alunoId);
        if (aluno == null)
            return;

        if(aluno.Avaliou)
            return;

        aluno.Avaliou = true;

        var avaliaramGrupo = grupo.Alunos.Count(a => a.Avaliou);

        grupo.Avaliaram = avaliaramGrupo;
        grupo.Pendentes = grupo.Alunos.Count - avaliaramGrupo;

        var todosAlunos = dto.Grupos.SelectMany(g => g.Alunos);

        var avaliaram = todosAlunos.Count(a => a.Avaliou);

        dto.Avaliaram = avaliaram;
        dto.Pendentes = dto.TotalAlunos - avaliaram;

        await SetAsync(sessaoId, dto);
    }

    static decimal AtualizarMedia(decimal mediaAtual, int totalAtual, decimal novaNota)
    {
        return ((mediaAtual * totalAtual) + novaNota) / (totalAtual + 1);
    }
}