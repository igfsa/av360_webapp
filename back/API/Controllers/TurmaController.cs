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
    [HttpGet("{id:int}", Name = "GetTurmaId")]
    [ActionName("GetId")]
    public async Task<ActionResult<TurmaDTO>> Get(int id)
    {
        var turma = await _turmaService.GetTurmaById(id);
        return Ok(turma);
    }

    [Authorize]
    [HttpGet("{alunoId:int}", Name = "GetTurmasAluno")]
    [ActionName("GetTurmasAluno")]
    public async Task<ActionResult<IEnumerable<TurmaDTO>>> GetTurmasAluno(int alunoId)
    {
        var turmas = await _turmaService.GetTurmasAluno(alunoId);
        return Ok(turmas);
    }

    [Authorize]
    [HttpGet("{criterioId:int}", Name = "GetTurmasCriterio")]
    [ActionName("GetTurmasCriterio")]
    public async Task<ActionResult<IEnumerable<TurmaDTO>>> GetCriteriosTurma(int criterioId)
    {
        var criterios = await _turmaService.GetTurmasCriterio(criterioId);
        return Ok(criterios);
    }

    [Authorize]
    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<TurmaDTO>> Post(TurmaDTO model)
    {
        var Turma = await _turmaService.Add(model);

        await _turmaNotifier.TurmaAtualizadaAsync(Turma.Id);
        return Ok(Turma);
    }

    [Authorize]
    [HttpPost("{turmaId:int}", Name = "AddAlunoTurma")]
    [ActionName("PostAlunoTurma")]
    public async Task<ActionResult<AlunoDTO>> PostAlunoTurma(int turmaId, int alunoId)
    {
        var aluno = await _turmaService.AddTurmaAluno(turmaId, alunoId);

        await _turmaNotifier.TurmaAtualizadaAsync(turmaId);
        return Ok(aluno);
    }

    [Authorize]
    [HttpPost("", Name = "ImportAlunosTurma")]
    [ActionName("ImportAlunosTurma")]
    public async Task<ActionResult<CsvImportResultDTO>> ImportAlunosTurma([FromForm] CsvImportRequestDTO dto)
    {
        var result = await _turmaService.ImportarAlunosAsync(dto.TurmaId, dto);
        
        await _turmaNotifier.TurmaAtualizadaAsync(dto.TurmaId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("", Name = "PutCriterioTurma")]
    [ActionName("PutCriterioTurma")]
    public async Task<ActionResult<TurmaDTO>> PutCriterioTurma(TurmaCriterioDTO model)
    {
        var turma = await _turmaService.AddTurmaCriterio(model);

        await _turmaNotifier.TurmaAtualizadaAsync(model.TurmaId);
        return Ok(turma);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, TurmaDTO model)
    {
        var turma = await _turmaService.Update(id, model);

        await _turmaNotifier.TurmaAtualizadaAsync(model.Id);
        return Ok(turma);
    }

}
