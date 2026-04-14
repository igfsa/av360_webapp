using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotaFinalController(INotaFinalService NotasFinaisService) : ControllerBase
{
    private readonly INotaFinalService _NotasFinaisService = NotasFinaisService;

    [Authorize]
    [HttpGet("{id:int}", Name = "GetNotaFinalId")]
    [ActionName("GetId")]
    public async Task<ActionResult<NotaFinalDTO>> Get(int id)
    {
        var NotaFinal = await _NotasFinaisService.GetById(id);
        return Ok(NotaFinal);
    }

    [Authorize]
    [HttpGet("{sessaoId:int}", Name = "GetNotasFinaisAlunoSessao")]
    [ActionName("GetNotasFinaisAlunoSessao")]
    public async Task<ActionResult<IEnumerable<NotaFinalDTO>>> GetNotasFinaisAlunoSessao(int alunoId, int sessaoId)
    {
        var NotasFinais = await _NotasFinaisService.GetNotasFinaisAlunoSessao(alunoId, sessaoId);
        return Ok(NotasFinais);
    }

    [Authorize]
    [HttpGet("{sessaoId:int}", Name = "GetNotasFinaisGrupoSessao")]
    [ActionName("GetNotasFinaisGrupoSessao")]
    public async Task<ActionResult<IEnumerable<NotaFinalDTO>>> GetNotasFinaisGrupoSessao(int grupoId, int sessaoId)
    {
        var NotasFinais = await _NotasFinaisService.GetNotasFinaisGrupoSessao(grupoId, sessaoId);
        return Ok(NotasFinais);
    }

    [Authorize]
    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<NotaFinalDTO>> Post(NotaFinalDTO model)
    {
        var NotaFinal = await _NotasFinaisService.Add(model);
        return Ok(NotaFinal);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, NotaFinalDTO model)
    {
        var NotaFinal = await _NotasFinaisService.Update(id, model);
        return Ok(NotaFinal);
    }

}
