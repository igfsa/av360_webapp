using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TurmaController(ITurmaService turmaService,
                    ITurmaNotifier turmaNotifier) : ControllerBase
{
    private readonly ITurmaService _turmaService = turmaService;
    private readonly ITurmaNotifier _turmaNotifier = turmaNotifier;

    [Authorize]
    [HttpGet]
    [ActionName("GetAllTurmas")]
    public async Task<ActionResult<IEnumerable<TurmaDTO>>> Get()
    {
        var turmas = await _turmaService.GetTurmas();
        return Ok(turmas);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    [ActionName("GetTurmaById")]
    public async Task<ActionResult<TurmaDTO>> GetTurmaById(int id)
    {
        var turma = await _turmaService.GetTurmaById(id);
        return Ok(turma);
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostTurma")]
    public async Task<ActionResult<TurmaDTO>> Post(TurmaDTO model)
    {
        var Turma = await _turmaService.Add(model);

        await _turmaNotifier.TurmaAtualizadaAsync(Turma.Id);
        return Ok(Turma);
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostImportAlunosTurma")]
    public async Task<ActionResult<CsvImportResultDTO>> ImportAlunosTurma([FromForm] CsvImportRequestDTO dto)
    {
        var result = await _turmaService.ImportarAlunosAsync(dto.TurmaId, dto);
        
        await _turmaNotifier.TurmaAtualizadaAsync(dto.TurmaId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [ActionName("PutCriterioTurma")]
    public async Task<ActionResult<TurmaDTO>> PutCriterioTurma(TurmaCriterioDTO model)
    {
        var turma = await _turmaService.AddTurmaCriterio(model);

        await _turmaNotifier.TurmaAtualizadaAsync(model.TurmaId);
        return Ok(turma);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ActionName("PutTurma")]
    public async Task<ActionResult> Put(int id, TurmaDTO model)
    {
        var turma = await _turmaService.Update(id, model);

        await _turmaNotifier.TurmaAtualizadaAsync(model.Id);
        return Ok(turma);
    }

}
