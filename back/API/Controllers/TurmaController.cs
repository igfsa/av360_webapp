using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TurmaController : ControllerBase
    {
        private readonly ITurmaService _turmaService;
        private readonly IAlunoService _alunoService;
        private ITurmaNotifier _turmaNotifier;
        public TurmaController(ITurmaService turmaService,
                            IAlunoService alunoService,
                            ITurmaNotifier turmaNotifier)
        {
            _turmaService = turmaService;
            _alunoService = alunoService;
            _turmaNotifier = turmaNotifier;
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Turma. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id:int}", Name = "GetTurmaId")]
        [ActionName("GetId")]
        public async Task<ActionResult<TurmaDTO>> Get(int id)
        {

            try
            {
                var turma = await _turmaService.GetTurmaById(id);
                return Ok(turma);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Turma. Erro: {ex.Message}");
            }

        }

        [HttpGet("{alunoId:int}", Name = "GetTurmasAluno")]
        [ActionName("GetTurmasAluno")]
        public async Task<ActionResult<IEnumerable<TurmaDTO>>> GetTurmasAluno(int alunoId)
        {

            try
            {
                var turmas = await _turmaService.GetTurmasAluno(alunoId);
                return Ok(turmas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Turmas. Erro: {ex.Message}");
            }

        }

        [HttpGet("{criterioId:int}", Name = "GetTurmasCriterio")]
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar critérios. Erro: {ex.Message}");
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Turma. Erro: {ex.Message}");
            }
        }

        [HttpPost("{turmaId:int}", Name = "AddAlunoTurma")]
        [ActionName("PostAlunoTurma")]
        public async Task<ActionResult<AlunoDTO>> PostAlunoTurma(int turmaId, int alunoId)
        {
            try
            {
                var turma = await _turmaService.GetTurmaById(turmaId);
                var aluno = await _turmaService.AddTurmaAluno(turmaId, alunoId);

                await _turmaNotifier.TurmaAtualizadaAsync(turmaId);
                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Aluno na Turma. Erro: {ex.Message}");
            }
        }
        [HttpPost("{turmaId:int}", Name = "ImportAlunosTurma")]
        [ActionName("ImportAlunosTurma")]
        public async Task<ActionResult<CsvImportResultDTO>> ImportAlunosTurma([FromForm] CsvImportRequestDTO dto)
        {
            try
            { 
                var result = await _turmaService.ImportarAlunosAsync(dto.TurmaId, dto);
                await _turmaNotifier.TurmaAtualizadaAsync(dto.TurmaId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar importar Alunos na Turma. Erro: {ex.Message}");
            }
        }

        [HttpPut("", Name = "PutCriterioTurma")]
        [ActionName("PutCriterioTurma")]
        public async Task<ActionResult<TurmaDTO>> PutCriterioTurma(TurmaCriterioDTO model)
        {
            try
            {
                var turma = await _turmaService.AddTurmaCriterio(model);
                await _turmaNotifier.TurmaAtualizadaAsync(model.turmaId);
                return Ok(turma);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Critérios na Turma. Erro: {ex.Message}");
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
