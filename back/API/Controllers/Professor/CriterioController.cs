using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CriterioController(
                    ICriterioService criterioService,
                    ICriterioNotifier criterioNotifier,
                    IAcessoService acessoService) : ControllerBase
{
    private readonly ICriterioService _criterioService = criterioService;
    private readonly ICriterioNotifier _criterioNotifier = criterioNotifier;
    private readonly IAcessoService _acessoService = acessoService;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ActionName("GetAllCriterios")]
    public async Task<ActionResult<IEnumerable<CriterioDTO>>> Get()
    {
        var criterios = await _criterioService.GetCriterios();
        return Ok(criterios);
    }

    [Authorize]
    [HttpGet("{turmaId:int}")]
    [ActionName("GetCriteriosTurma")]
    public async Task<ActionResult<CriterioDTO>> GetCriterioTurma(int turmaId)
    {
        await _acessoService.GarantirTurmaAsync(turmaId);
        var criterios = await _criterioService.GetCriteriosTurma(turmaId);
        return Ok(criterios);
    }

    [Authorize]
    [HttpGet]
    [ActionName("GetCriteriosProfessor")]
    public async Task<ActionResult<CriterioDTO>> GetCriteriosProfessor()
    {
        var criterios = await _criterioService.GetCriteriosProfessor();
        return Ok(criterios);
    }

    [Authorize]
    [HttpGet]
    [ActionName("GetCriteriosSugestoes")]
    public async Task<ActionResult<CriterioDTO>> GetCriteriosSugestoes()
    {
        var criterios = await _criterioService.GetCriteriosSugestoes();
        return Ok(criterios);
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostCriterio")]
    public async Task<ActionResult<CriterioDTO>> Post(CriterioDTO model)
    {
        var criterio = await _criterioService.Add(model);

        await _criterioNotifier.CriterioAtualizado(criterio.Id);
        return Ok(criterio);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ActionName("PutCriterio")]
    public async Task<ActionResult> Put(int id, CriterioDTO model)
    {
        await _acessoService.GarantirCriterioAsync(id);
        var criterio = await _criterioService.Update(id, model);

        await _criterioNotifier.CriterioAtualizado(model.Id);
        return Ok(criterio);
    }

}
