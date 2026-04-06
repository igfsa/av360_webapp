using System.Text.Json;
using StackExchange.Redis;

using Application.Contracts;
using Application.DTOs;

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

    public async Task AtualizarNotaAsync(
        int sessaoId,
        int alunoId,
        int criterioId,
        int grupoId,
        decimal nota)
    {
        static decimal AtualizarMedia(decimal mediaAtual, int totalAtual, decimal novaNota)
        {
            return ((mediaAtual * totalAtual) + novaNota) / (totalAtual + 1);
        }

        var dto = await GetAsync(sessaoId);

        if (dto == null)
            return; 
        
        var aluno = dto.Alunos.First(a => a.AlunoId == alunoId);

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

        var grupo = dto.Grupos.First(g => g.GrupoId == grupoId);

        grupo.Media = AtualizarMedia(
            grupo.Media,
            grupo.TotalNotas,
            nota);

        grupo.TotalNotas++;

        var avaliaram = dto.Alunos.Count(a => a.Avaliou);

        dto.Avaliaram = avaliaram;
        dto.Pendentes = dto.TotalAlunos - avaliaram;

        await SetAsync(sessaoId, dto);
    }

    public async Task AtualizarAlunoAsync(int sessaoId, int alunoId)
    {
        var dto = await GetAsync(sessaoId);
        if (dto == null)
            return; 

        var aluno = dto.Alunos.FirstOrDefault(a => a.AlunoId == alunoId);
        if (aluno == null)
            return; 

        aluno.Avaliou = true;

        await SetAsync(sessaoId, dto);
    }

}