using Application.DTOs;

namespace Application.Contracts;

public interface IDashboardSessaoService
{
    Task<DashboardSessaoDTO?> GetDashboard(int sessaoId);
}