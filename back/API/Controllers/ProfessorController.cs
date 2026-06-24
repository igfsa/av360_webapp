using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProfessorController(
                    IProfessorService professorService) : ControllerBase
{
    private readonly IProfessorService _professorService = professorService;

    [Authorize]
    [HttpGet]
    [ActionName("GetAllProfessores")]
    public async Task<ActionResult<IEnumerable<ProfessorDTO>>> Get()
    {
        var professors = await _professorService.GetProfessores();
        return Ok(professors);
    }
}