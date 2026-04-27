using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DashboardController(
                    IDashboardSessaoService dashboardService) : ControllerBase
{
    private readonly IDashboardSessaoService _dashboardService = dashboardService;

    [Authorize]
    [HttpGet("{sessaoId:int}")]
    [ActionName("GetDashboard")]
    public async Task<ActionResult<DashboardSessaoDTO>> GetDashboard(int sessaoId)
    {
        var result = await _dashboardService.GetDashboard(sessaoId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{sessaoId:int}")]
    [ActionName("PostDashboardReset")]
    public async Task<ActionResult<DashboardSessaoDTO>> PostDashboardReset(int sessaoId)
    {
        var result = await _dashboardService.ResetDashboard(sessaoId);
        return Ok(result);
    }

}
