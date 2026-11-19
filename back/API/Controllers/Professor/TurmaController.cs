using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TurmaController(ITurmaService turmaService,
                    ITurmaNotifier turmaNotifier,
                    IAcessoService acessoService
                    ) : ControllerBase
{
    private readonly ITurmaService _turmaService = turmaService;
    private readonly IAcessoService _acessoService = acessoService;
    private readonly ITurmaNotifier _turmaNotifier = turmaNotifier;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ActionName("GetAllTurmasAdm")]
    public async Task<ActionResult<IEnumerable<TurmaAdmDTO>>> Get()
    {
        var turmas = await _turmaService.GetTurmasAdm();
        return Ok(turmas);
    }

    [Authorize]
    [HttpGet]
    [ActionName("GetTurmasProfessorId")]
    public async Task<ActionResult<IEnumerable<TurmaDTO>>> GetTurmasProfessorId()
    {
        var turmas = await _turmaService.GetTurmasProfessorId();
        return Ok(turmas);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    [ActionName("GetTurmaById")]
    public async Task<ActionResult<TurmaDTO>> GetTurmaById(int id)
    {
        await _acessoService.GarantirTurmaAsync(id);
        var turma = await _turmaService.GetTurmaById(id);
        return Ok(turma);
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostTurma")]
    public async Task<ActionResult<TurmaDTO>> Post(TurmaDTO model)
    {
        var Turma = await _turmaService.Add(model);

        await _turmaNotifier.TurmaAtualizada(Turma.Id);
        return Ok(Turma);
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostImportAlunosTurma")]
    public async Task<ActionResult<CsvImportResultDTO>> ImportAlunosTurma([FromForm] CsvImportRequestDTO dto)
    {
        await _acessoService.GarantirTurmaAsync(dto.TurmaId);

        var result = await _turmaService.ImportarAlunosAsync(dto.TurmaId, dto);
        
        await _turmaNotifier.TurmaAtualizada(dto.TurmaId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [ActionName("PutCriterioTurma")]
    public async Task<ActionResult<TurmaDTO>> PutCriterioTurma(TurmaCriterioDTO model)
    {
        await _acessoService.GarantirTurmaAsync(model.TurmaId);
        var turma = await _turmaService.AddTurmaCriterio(model);

        await _turmaNotifier.TurmaAtualizada(model.TurmaId);
        return Ok(turma);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ActionName("PutTurma")]
    public async Task<ActionResult> Put(int id, TurmaDTO model)
    {
        await _acessoService.GarantirTurmaAsync(id);
        var turma = await _turmaService.Update(id, model);

        await _turmaNotifier.TurmaAtualizada(model.Id);
        return Ok(turma);
    }

    [Authorize]
    [HttpPut("{turmaId:int}")]
    [ActionName("RemoverProfessor")]
    public async Task<ActionResult> RemoverProfessor(int turmaId)
    {
        await _acessoService.GarantirTurmaAsync(turmaId);
        var turma = await _turmaService.RemoverProfessor(turmaId);

        await _turmaNotifier.TurmaAtualizada(turmaId);
        return Ok(turma);
    }

    [Authorize(Roles = "Admin")]
    [Authorize]
    [HttpPut("{turmaId:int}")]
    [ActionName("AlterarProfessor")]
    public async Task<ActionResult> AlterarProfessor(int turmaId, [FromQuery] int professorId)
    {
        var turma = await _turmaService.AlterarProfessor(turmaId, professorId);

        await _turmaNotifier.TurmaAtualizada(turmaId);
        return Ok(turma);
    }

}
