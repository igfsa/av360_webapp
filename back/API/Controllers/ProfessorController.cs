using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProfessorController(
                    IProfessorService professorService,
                    IProfessorNotifier professorNotifier) : ControllerBase
{
    private readonly IProfessorService _professorService = professorService;
    private readonly IProfessorNotifier _professorNotifier = professorNotifier;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ActionName("GetAllProfessores")]
    public async Task<ActionResult<IEnumerable<ProfessorDTO>>> Get()
    {
        var professors = await _professorService.GetProfessores();
        return Ok(professors);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    [ActionName("PutProfessor")]
    public async Task<ActionResult> Put(int id, ProfessorDTO model)
    {
        var professor = await _professorService.Update(id, model);

        await _professorNotifier.ProfessorAtualizado(model.Id);
        return Ok(professor);
    }
}