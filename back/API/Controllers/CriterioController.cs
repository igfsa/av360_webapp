using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CriterioController : ControllerBase
    {
        private readonly ICriterioService _criterioService;

        public CriterioController(ICriterioService criterioService)
        {
            _criterioService = criterioService;
        }

        [HttpGet]
        [ActionName("GetAllCriterios")]
        public async Task<ActionResult<IEnumerable<CriterioDTO>>> Get()
        {
            try
            {
                var criterios = await _criterioService.GetCriterios();
                if (criterios is null)
                {
                    return NotFound();
                }
                return Ok(criterios);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar critérios. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCriterioId")]
        [ActionName("GetId")]
        public async Task<ActionResult<CriterioDTO>> Get(int id)
        {

            try
            {
                var criterios = await _criterioService.GetCriterioById(id);
                if (criterios is null)
                {
                    return NotFound();
                }
                return Ok(criterios);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar critério. Erro: {ex.Message}");
            }

        }

        [HttpGet("{turmaId:int}", Name = "ObterCriterioTurma")]
        [ActionName("ObterCriterioTurma")]
        public async Task<ActionResult<CriterioDTO>> GetCriterioTurma(int turmaId)
        {

            try
            {
                var criterios = await _criterioService.GetCriteriosTurma(turmaId);
                if (criterios is null)
                {
                    return NotFound();
                }
                return Ok(criterios);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar critério. Erro: {ex.Message}");
            }

        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult<CriterioDTO>> Post(CriterioDTO model)
        {
            try
            {
                var criterio = await _criterioService.Add(model);
                if (model == null) 
                {
                   return NoContent(); 
                }
                return Ok(criterio);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar critério. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(int id, CriterioDTO model)
        {
            try
            {
                var criterio = await _criterioService.Update(id, model);
                if (criterio == null)
                {
                   return NoContent(); 
                }
                return Ok(criterio);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar critério. Erro: {ex.Message}");
            }
        }

    }

}
