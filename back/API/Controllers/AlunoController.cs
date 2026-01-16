using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunoController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [ActionName("GetAllAlunos")]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> Get()
        {
            try
            {
                var alunos = await _alunoService.GetAlunos();
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

        [HttpGet("{id:int}", Name = "ObterAlunoId")]
        [ActionName("GetId")]
        public async Task<ActionResult<AlunoDTO>> Get(int id)
        {

            try
            {
                var alunos = await _alunoService.GetAlunoById(id);
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

        [HttpGet("{turmaId:int}", Name = "ObterAlunosTurma")]
        [ActionName("GetAlunosTurma")]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunosTurma(int turmaId)
        {

            try
            {
                var turmas = await _alunoService.GetAlunosTurma(turmaId);
                if (turmas is null)
                {
                    return NotFound();
                }
                return Ok(turmas);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar aluno. Erro: {ex.Message}");
            }

        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult<AlunoDTO>> Post(AlunoDTO model)
        {
            try
            {
                var aluno = await _alunoService.Add(model);
                if (model == null) 
                {
                   return NoContent(); 
                }
                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar aluno. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(int id, AlunoDTO model)
        {
            try
            {
                var aluno = await _alunoService.Update(id, model);
                if (aluno == null)
                {
                   return NoContent(); 
                }
                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar alunos. Erro: {ex.Message}");
            }
        }

    }

}
