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
        private readonly ICriterioService _criterioService;
        private ITurmaNotifier _turmaNotifier;
        public TurmaController(ITurmaService turmaService,
                            IAlunoService alunoService,
                            ICriterioService criterioService,
                            ITurmaNotifier turmaNotifier)
        {
            _turmaService = turmaService;
            _alunoService = alunoService;
            _turmaNotifier = turmaNotifier;
            _criterioService = criterioService;
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

        [HttpGet("{alunoId:int}", Name = "ObterTurmasAluno")]
        [ActionName("GetTurmasAluno")]
        public async Task<ActionResult<IEnumerable<TurmaDTO>>> GetTurmasAluno(int alunoId)
        {

            try
            {
                var alunos = await _turmaService.GetTurmasAluno(alunoId);
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

        [HttpGet("{criterioId:int}", Name = "ObterTurmasCriterio")]
        [ActionName("GetTurmasCriterio")]
        public async Task<ActionResult<IEnumerable<TurmaDTO>>> GetCriteriosTurma(int criterioId)
        {

            try
            {
                var criterios = await _turmaService.GetTurmasCriterio(criterioId);
                if (criterios is null)
                {
                    return NotFound();
                }
                return Ok(criterios);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar criterio. Erro: {ex.Message}");
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
                await _turmaNotifier.TurmaAtualizadaAsync(Turma.Id);
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
                await _turmaNotifier.TurmaAtualizadaAsync(turmaId);
                return Ok($"Aluno {aluno.Nome} adicionado na turma {turma.Cod}.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Turma. Erro: {ex.Message}");
            }
        }

        [HttpPost("{criterioId:int}", Name = "AddCriterioTurma")]
        [ActionName("PostCriterioTurma")]
        public async Task<ActionResult<CriterioDTO>> PostCriterioTurma(int criterioId, int turmaId)
        {
            try
            {
                var criterio = await _criterioService.GetCriterioById(criterioId);
                var turma = await _turmaService.GetTurmaById(turmaId);
                var addTurma = await _turmaService.AddTurmaCriterio(criterioId, turmaId);
                
                if (addTurma == null) 
                {
                    return BadRequest($"Criterio {criterio.Nome} já existe na turma {turma.Cod}.");
                }
                await _turmaNotifier.TurmaAtualizadaAsync(turmaId);
                return Ok($"Criterio {criterio.Nome} adicionado na turma {turma.Cod}.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
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
                await _turmaNotifier.TurmaAtualizadaAsync(model.Id);
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
