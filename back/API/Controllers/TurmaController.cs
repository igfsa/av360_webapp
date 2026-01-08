using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TurmaController : ControllerBase
    {
        private readonly ITurmaService _turmaService;
        private readonly IAlunoService _alunoService;
        public TurmaController(ITurmaService turmaService,
                            IAlunoService alunoService)
        {
            _turmaService = turmaService;
            _alunoService = alunoService;
        }

        [HttpGet]
        [ActionName("GetAllTurmas")]
        public async Task<ActionResult<IEnumerable<TurmaDTO>>> Get()
        {
            try
            {
                var turmas = await _turmaService.GetTurmas();
                if (turmas is null)
                {
                    return NotFound();
                }
                return Ok(turmas);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Turma. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id:int}", Name = "ObterTurmaId")]
        [ActionName("GetId")]
        public async Task<ActionResult<TurmaDTO>> Get(int id)
        {

            try
            {
                var turmas = await _turmaService.GetTurmaById(id);
                if (turmas is null)
                {
                    return NotFound();
                }
                return Ok(turmas);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Turma. Erro: {ex.Message}");
            }

        }

        [HttpGet("{turmaId:int}", Name = "ObterTurmaAluno")]
        [ActionName("GetTurmaAluno")]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetTurmaAluno(int turmaId)
        {

            try
            {
                var alunos = await _turmaService.GetTurmaAluno(turmaId);
                if (alunos is null)
                {
                    return NotFound();
                }
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar aluno. Erro: {ex.Message}");
            }

        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult<TurmaDTO>> Post(TurmaDTO model)
        {
            try
            {
                var Turma = await _turmaService.Add(model);
                if (model == null) 
                {
                   return NoContent(); 
                }
                return Ok(Turma);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Turma. Erro: {ex.Message}");
            }
        }

        [HttpPost("{alunoId:int}", Name = "AddAlunoTurma")]
        [ActionName("PostAlunoTurma")]
        public async Task<ActionResult<AlunoDTO>> PostAlunoTurma(int alunoId, int turmaId)
        {
            try
            {
                var aluno = await _alunoService.GetAlunoById(alunoId);
                var turma = await _turmaService.GetTurmaById(turmaId);
                var addTurma = await _turmaService.AddTurmaAluno(alunoId, turmaId);
                
                if (addTurma == null) 
                {
                    return BadRequest($"Aluno {aluno.Nome} já existe na turma {turma.Cod}.");
                }
                return Ok($"Aluno {aluno.Nome} adicionado na turma {turma.Cod}.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Turma. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(int id, TurmaDTO model)
        {
            try
            {
                var turma = await _turmaService.Update(id, model);
                if (turma == null)
                {
                   return NoContent(); 
                }
                return Ok(turma);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar Turmas. Erro: {ex.Message}");
            }
        }

    }

}
