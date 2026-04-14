using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GrupoController(
    IGrupoService grupoService,
    IAlunoService alunoService,
    ITurmaNotifier turmaNotifier,
    IGrupoNotifier grupoNotifier) : ControllerBase
{
    private readonly IGrupoService _grupoService = grupoService;
    private readonly IAlunoService _alunoService = alunoService;
    private readonly ITurmaNotifier _turmaNotifier = turmaNotifier;
    private readonly IGrupoNotifier _grupoNotifier = grupoNotifier;

    [Authorize]
    [HttpGet]
    [ActionName("GetAllGrupos")]
    public async Task<ActionResult<IEnumerable<GrupoDTO>>> Get()
    {
        var grupos = await _grupoService.GetGrupos();
        return Ok(grupos);
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "GetGrupoId")]
    [ActionName("GetId")]
    public async Task<ActionResult<GrupoDTO>> Get(int id)
    {
        var grupos = await _grupoService.GetGrupoById(id);
        return Ok(grupos);
    }

    [Authorize]
    [HttpGet("{turmaId:int}", Name = "GetGruposTurma")]
    [ActionName("GetGruposTurma")]
    public async Task<ActionResult<IEnumerable<GrupoDTO>>> GetGruposTurma(int turmaId)
    {
        var grupos = await _grupoService.GetGruposTurma(turmaId);
        return Ok(grupos);
    }

    [Authorize]
    [HttpGet("{turmaId:int}", Name = "GetAlunoGruposCheckbox")]
    [ActionName("GetAlunoGruposCheckbox")]
    public async Task<ActionResult<IEnumerable<AlunoGrupoCheckboxDTO>>> GetAlunosGrupoCheckbox(int turmaId, int grupoId)
    {
        var alunosCheckbox = await _grupoService.GetAlunoGrupoTurma(turmaId, grupoId);
        return Ok(alunosCheckbox);
        
    }

    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<GrupoDTO>> Post(GrupoDTO model)
    {
        var grupo = await _grupoService.Add(model);

        await _grupoNotifier.GrupoAtualizadoAsync(grupo.Id);
        await _turmaNotifier.TurmaAtualizadaAsync(grupo.TurmaId);
        return Ok(grupo);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, GrupoDTO model)
    {
        var grupo = await _grupoService.Update(id, model);

        await _grupoNotifier.GrupoAtualizadoAsync(grupo.Id);
        await _turmaNotifier.TurmaAtualizadaAsync(grupo.TurmaId);
        return Ok(grupo);
    }

    [Authorize]
    [HttpPut("", Name = "PutAtualizarGrupo")]
    [ActionName("PutAtualizarGrupo")]
    public async Task<ActionResult<GrupoDTO>> PutAtualizarGrupo(AlunoGrupoDTO model)
    {
            await _grupoService.AtualizarGrupo(model.TurmaId, model.GrupoId, model.AlunoIds);

            await _grupoNotifier.GrupoAtualizadoAsync(model.GrupoId);
            await _turmaNotifier.TurmaAtualizadaAsync(model.TurmaId);
            var grupo = await _grupoService.GetGrupoById(model.GrupoId);
            return Ok(grupo);
    }
}
