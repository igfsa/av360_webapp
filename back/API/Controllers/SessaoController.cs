using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SessaoController : ControllerBase
    {
        private readonly ISessaoService _SessoesService;

        public SessaoController(ISessaoService SessoesService)
        {
            _SessoesService = SessoesService;
        }

        [HttpGet]
        [ActionName("GetAllSessoes")]
        public async Task<ActionResult<IEnumerable<SessaoDTO>>> Get()
        {
            try
            {
                var Sessoes = await _SessoesService.GetSessoes();
                if (Sessoes is null)
                {
                    return NotFound();
                }
                return Ok(Sessoes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seções. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id:int}", Name = "GetSessaoId")]
        [ActionName("GetId")]
        public async Task<ActionResult<SessaoDTO>> Get(int id)
        {

            try
            {
                var Sessao = await _SessoesService.GetSessaoById(id);
                if (Sessao is null)
                {
                    return NotFound();
                }
                return Ok(Sessao);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seção. Erro: {ex.Message}");
            }

        }

        [HttpGet("{turmaId:int}", Name = "GetSessoesTurma")]
        [ActionName("GetSessoesTurma")]
        public async Task<ActionResult<IEnumerable<SessaoDTO>>> GetSessoesTurma(int turmaId)
        {

            try
            {
                var Sessoes = await _SessoesService.GetSessoesTurma(turmaId);
                if (Sessoes is null)
                {
                    return NotFound();
                }
                return Ok(Sessoes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seções. Erro: {ex.Message}");
            }

        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult<SessaoDTO>> Post(SessaoDTO model)
        {
            try
            {
                var Sessao = await _SessoesService.Add(model);
                if (model == null) 
                {
                   return NoContent(); 
                }
                return Ok(Sessao);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Seção. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(int id, SessaoDTO model)
        {
            try
            {
                var Sessao = await _SessoesService.Update(id, model);
                if (Sessao == null)
                {
                   return NoContent(); 
                }
                return Ok(Sessao);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar Seções. Erro: {ex.Message}");
            }
        }

    }

}
