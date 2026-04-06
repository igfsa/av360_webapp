using Microsoft.AspNetCore.Mvc;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CriterioController(
                    ICriterioService criterioService,
                    ICriterioNotifier criterioNotifier) : ControllerBase
{
    private readonly ICriterioService _criterioService = criterioService;
    private readonly ICriterioNotifier _criterioNotifier = criterioNotifier;

    [HttpGet]
    [ActionName("GetAllCriterios")]
    public async Task<ActionResult<IEnumerable<CriterioDTO>>> Get()
    {
        var criterios = await _criterioService.GetCriterios();
        return Ok(criterios);
    }

    [HttpGet("{id:int}", Name = "GetCriterioId")]
    [ActionName("GetId")]
    public async Task<ActionResult<CriterioDTO>> Get(int id)
    {
        var criterios = await _criterioService.GetCriterioById(id);
        return Ok(criterios);
    }

    [HttpGet("{turmaId:int}", Name = "GetCriteriosTurma")]
    [ActionName("GetCriteriosTurma")]
    public async Task<ActionResult<CriterioDTO>> GetCriterioTurma(int turmaId)
    {
        var criterios = await _criterioService.GetCriteriosTurma(turmaId);

        return Ok(criterios);
    }

    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<CriterioDTO>> Post(CriterioDTO model)
    {
        var criterio = await _criterioService.Add(model);

        await _criterioNotifier.CriterioAtualizadoAsync(criterio.Id);
        return Ok(criterio);
    }

    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, CriterioDTO model)
    {
        var criterio = await _criterioService.Update(id, model);

        await _criterioNotifier.CriterioAtualizadoAsync(model.Id);
        return Ok(criterio);
    }

}
