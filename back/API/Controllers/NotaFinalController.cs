using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotaFinalController : ControllerBase
    {
        private readonly INotaFinalService _NotasFinaisService;

        public NotaFinalController(INotaFinalService NotasFinaisService)
        {
            _NotasFinaisService = NotasFinaisService;
        }

        [HttpGet("{id:int}", Name = "GetNotaFinalId")]
        [ActionName("GetId")]
        public async Task<ActionResult<NotaFinalDTO>> Get(int id)
        {

            try
            {
                var NotaFinal = await _NotasFinaisService.GetById(id);
                if (NotaFinal is null)
                {
                    return NotFound();
                }
                return Ok(NotaFinal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seção. Erro: {ex.Message}");
            }

        }

        [HttpGet("{turmaId:int}", Name = "GetNotasFinaisAlunoSessao")]
        [ActionName("GetNotasFinaisAlunoSessao")]
        public async Task<ActionResult<IEnumerable<NotaFinalDTO>>> GetNotasFinaisAlunoSessao(int alunoId, int sessaoId) {
            try {
                var NotasFinais = await _NotasFinaisService.GetNotasFinaisAlunoSessao(alunoId, sessaoId);
                if (NotasFinais is null)
                {
                    return NotFound();
                }
                return Ok(NotasFinais);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seções. Erro: {ex.Message}");
        }}

        [HttpGet("{turmaId:int}", Name = "GetNotasFinaisGrupoSessao")]
        [ActionName("GetNotasFinaisGrupoSessao")]
        public async Task<ActionResult<IEnumerable<NotaFinalDTO>>> GetNotasFinaisGrupoSessao(int grupoId, int sessaoId) {
            try {
                var NotasFinais = await _NotasFinaisService.GetNotasFinaisGrupoSessao(grupoId, sessaoId);
                if (NotasFinais is null)
                {
                    return NotFound();
                }
                return Ok(NotasFinais);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seções. Erro: {ex.Message}");
        }}

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult<NotaFinalDTO>> Post(NotaFinalDTO model) {
            try {
                var NotaFinal = await _NotasFinaisService.Add(model);
                if (model == null) 
                {
                   return NoContent(); 
                }
                return Ok(NotaFinal);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Seção. Erro: {ex.Message}");
        }}

        [HttpPut("{id:int}")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(int id, NotaFinalDTO model)
        {
            try
            {
                var NotaFinal = await _NotasFinaisService.Update(id, model);
                if (NotaFinal == null)
                {
                   return NoContent(); 
                }
                return Ok(NotaFinal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar Seções. Erro: {ex.Message}");
            }
        }

    }

}
