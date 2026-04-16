using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AlunoController(IAlunoService alunoService) : ControllerBase
{
    private readonly IAlunoService _alunoService = alunoService;

    [Authorize]
    [HttpGet]
    [ActionName("GetAllAlunos")]
    public async Task<ActionResult<IEnumerable<AlunoDTO>>> Get()
    {
        var alunos = await _alunoService.GetAlunos();
        return Ok(alunos);
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "ObterAlunoId")]
    [ActionName("GetId")]
    public async Task<ActionResult<AlunoDTO>> Get(int id)
    {
        var alunos = await _alunoService.GetAlunoById(id);
        return Ok(alunos);
    }

    [AllowAnonymous]
    [HttpGet("{grupoId:int}", Name = "ObterAlunoNomeIdGrupo")]
    [ActionName("ObterAlunoNomeIdGrupo")]
    public async Task<ActionResult<AlunoDTO>> GetAlunoByNomeIdGrupo(int grupoId, string nome)
    {
        var alunos = await _alunoService.GetAlunoByNomeIdGrupo(nome, grupoId);
        return Ok(alunos);
    }

    [Authorize]
    [HttpGet("{turmaId:int}", Name = "ObterAlunosTurma")]
    [ActionName("GetAlunosTurma")]
    public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunosTurma(int turmaId)
    {
        var aluno = await _alunoService.GetAlunosTurma(turmaId);
        return Ok(aluno);
    }

    [Authorize]
    [HttpGet("{turmaId:int}", Name = "GetAlunoGrupoNome")]
    [ActionName("GetAlunoGrupoNome")]
    public async Task<ActionResult<IEnumerable<AlunoGrupoNomeDTO>>> GetAlunoGrupoNome(int turmaId)
    {
        var alunos = await _alunoService.GetAlunoGrupoNome(turmaId);
        return Ok(alunos);
    }

    [Authorize]
    [HttpGet("{grupoId:int}", Name = "GetAlunosGrupo")]
    [ActionName("GetAlunosGrupo")]
    public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunosGrupo(int grupoId)
    {
        var alunos = await _alunoService.GetAlunosGrupo(grupoId);
        return Ok(alunos);
    }

    [Authorize]
    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<AlunoDTO>> Post(AlunoDTO model)
    {
        var aluno = await _alunoService.Add(model);
        return Ok(aluno);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, AlunoDTO model)
    {
        var aluno = await _alunoService.Update(id, model);
        return Ok(aluno);
    }
}
