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
                return StatusCode(StatusCodes.Status500InternalServerError,
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar aluno. Erro: {ex.Message}");
            }

        }

        [HttpGet("{grupoId:int}", Name = "ObterAlunoNomeIdGrupo")]
        [ActionName("ObterAlunoNomeIdGrupo")]
        public async Task<ActionResult<AlunoDTO>> GetAlunoByNomeIdGrupo(int grupoId, string nome)
        {

            try
            {
                var alunos = await _alunoService.GetAlunoByNomeIdGrupo(nome, grupoId);
                if (alunos is null)
                {
                    return NotFound();
                }
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar aluno. Erro: {ex.Message}");
            }

        }

        [HttpGet("{turmaId:int}", Name = "ObterAlunosTurma")]
        [ActionName("GetAlunosTurma")]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunosTurma(int turmaId)
        {

            try
            {
                var aluno = await _alunoService.GetAlunosTurma(turmaId);
                if (aluno is null)
                {
                    return NotFound();
                }
                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar aluno. Erro: {ex.Message}");
            }

        }
        
        [HttpGet("{turmaId:int}", Name = "GetAlunoGrupoNome")]
        [ActionName("GetAlunoGrupoNome")]
        public async Task<ActionResult<IEnumerable<AlunoGrupoNomeDTO>>> GetAlunoGrupoNome(int turmaId)
        {

            try
            {
                var alunos = await _alunoService.GetAlunoGrupoNome(turmaId);
                if (alunos is null)
                {
                    return NotFound();
                }
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar alunos. Erro: {ex.Message}");
            }

        }
        
        [HttpGet("{grupoId:int}", Name = "GetAlunosGrupo")]
        [ActionName("GetAlunosGrupo")]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunosGrupo(int grupoId)
        {

            try
            {
                var alunos = await _alunoService.GetAlunosGrupo(grupoId);
                if (alunos is null)
                {
                    return NotFound();
                }
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar alunos. Erro: {ex.Message}");
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
                return StatusCode(StatusCodes.Status500InternalServerError,
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
