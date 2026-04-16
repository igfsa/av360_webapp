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
    public async Task<ActionResult<DashboardSessaoDTO>> GetDashboard(int sessaoId)
    {
        var result = await _dashboardService.GetDashboard(sessaoId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{sessaoId:int}")]
    public async Task<ActionResult<DashboardSessaoDTO>> Reset(int sessaoId)
    {
        var result = await _dashboardService.ResetDashboard(sessaoId);
        return Ok(result);
    }

}
