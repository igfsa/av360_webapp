using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AlunoController(IAlunoService alunoService,
                    ITurmaNotifier turmaNotifier) : ControllerBase
{
    private readonly IAlunoService _alunoService = alunoService;
    private readonly ITurmaNotifier _turmaNotifier = turmaNotifier;

    [AllowAnonymous]
    [HttpGet("{grupoId:int}")]
    [ActionName("GetAlunoNomeIdGrupo")]
    public async Task<ActionResult<AlunoDTO>> GetAlunoByNomeIdGrupo(int grupoId, string nome)
    {
        var alunos = await _alunoService.GetAlunoByNomeIdGrupo(nome, grupoId);
        return Ok(alunos);
    }

    [Authorize]
    [HttpGet("{turmaId:int}")]
    [ActionName("GetAlunosTurma")]
    public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunosTurma(int turmaId)
    {
        var aluno = await _alunoService.GetAlunosTurma(turmaId);
        return Ok(aluno);
    }

    [Authorize]
    [HttpGet("{turmaId:int}")]
    [ActionName("GetAlunoGrupoNome")]
    public async Task<ActionResult<IEnumerable<AlunoGrupoNomeDTO>>> GetAlunoGrupoNome(int turmaId)
    {
        var alunos = await _alunoService.GetAlunoGrupoNome(turmaId);
        return Ok(alunos);
    }

    [Authorize]
    [HttpPost("{turmaId:int}")]
    [ActionName("PostAlunoTurma")]
    public async Task<ActionResult<AlunoDTO>> PostAlunoTurma(int turmaId, AlunoDTO alunoModel)
    {
        var aluno = await _alunoService.AddAlunoTurma(turmaId, alunoModel);

        await _turmaNotifier.TurmaAtualizadaAsync(turmaId);
        return Ok(aluno);
    }
}
