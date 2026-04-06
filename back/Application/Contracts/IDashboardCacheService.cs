using Application.DTOs;

namespace Application.Contracts;

public interface IDashboardCacheService
{
    Task<DashboardSessaoDTO?> GetAsync(int sessaoId);
    Task SetAsync(int sessaoId, DashboardSessaoDTO dto);
    Task AtualizarNotaAsync(int sessaoId, int alunoId, int criterioId, int grupoId, decimal nota);
    Task AtualizarAlunoAsync(int sessaoId, int alunoId);
}