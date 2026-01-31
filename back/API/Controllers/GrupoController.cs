using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;
using Domain.Entities;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GrupoController : ControllerBase
{
    private readonly IGrupoService _grupoService;
    private readonly IAlunoService _alunoService;
    private ITurmaNotifier _turmaNotifier;
    private IGrupoNotifier _grupoNotifier;

    public GrupoController(
        IGrupoService grupoService,
        IAlunoService alunoService,
        ITurmaNotifier turmaNotifier,
        IGrupoNotifier grupoNotifier)
    {
        _grupoService = grupoService;
        _alunoService = alunoService;
        _turmaNotifier = turmaNotifier;
        _grupoNotifier = grupoNotifier;
    }

    [HttpGet]
    [ActionName("GetAllGrupos")]
    public async Task<ActionResult<IEnumerable<GrupoDTO>>> Get()
    {
        try
        {
            var grupos = await _grupoService.GetGrupos();
            if (grupos is null)
            {
                return NotFound();
            }
            return Ok(grupos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar buscar Grupo. Erro: {ex.Message}");
        }
    }

    [HttpGet("{id:int}", Name = "GetGrupoId")]
    [ActionName("GetId")]
    public async Task<ActionResult<GrupoDTO>> Get(int id)
    {

        try
        {
            var grupos = await _grupoService.GetGrupoById(id);
            if (grupos is null)
            {
                return NotFound();
            }
            return Ok(grupos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar buscar Grupo. Erro: {ex.Message}");
        }

    }

    [HttpGet("{turmaId:int}", Name = "GetGruposTurma")]
    [ActionName("GetGruposTurma")]
    public async Task<ActionResult<IEnumerable<GrupoDTO>>> GetGruposTurma(int turmaId)
    {

        try
        {
            var grupos = await _grupoService.GetGruposTurma(turmaId);
            if (grupos is null)
            {
                return NotFound();
            }
            return Ok(grupos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar buscar Grupo. Erro: {ex.Message}");
        }

    }

    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<GrupoDTO>> Post(GrupoDTO model)
    {
        try
        {
            var grupo = await _grupoService.Add(model);
            if (model == null) 
            {
                return NoContent(); 
            }
            await _grupoNotifier.GrupoAtualizadoAsync(grupo.Id);
            await _turmaNotifier.TurmaAtualizadaAsync(grupo.TurmaId);
            return Ok(grupo);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar Grupo. Erro: {ex.Message}");
        }
    }
    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, GrupoDTO model)
    {
        try
        {
            var grupo = await _grupoService.Update(id, model);
            if (grupo == null)
            {
                return NoContent(); 
            }
            await _grupoNotifier.GrupoAtualizadoAsync(grupo.Id);
            await _turmaNotifier.TurmaAtualizadaAsync(grupo.TurmaId);
            return Ok(grupo);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                            $"Erro ao tentar atualizar Grupos. Erro: {ex.Message}");
        }
    }
    [HttpPut("", Name = "PutAlunoGrupo")]
    [ActionName("PutAlunoGrupo")]
    public async Task<ActionResult<GrupoDTO>> PutAlunoGrupo(AlunoGrupoDTO model)
    {
        try
        {
            var turma = await _grupoService.AddAlunoGrupo(model);
            await _turmaNotifier.TurmaAtualizadaAsync(model.grupoId);
            return Ok(turma);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar Grupo. Erro: {ex.Message}");
        }
    }
}
