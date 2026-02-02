using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly IAvaliacaoNotifier _avaliacaoNotifier;

        public AvaliacaoController(
            IAvaliacaoService avaliacaoService,
            IAvaliacaoNotifier avaliacaoNotifier)
        {
            _avaliacaoService = avaliacaoService;
            _avaliacaoNotifier = avaliacaoNotifier;
        }

        [HttpPut("")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(AvaliacaoEnvioDTO model)
        {
            try
            {
                var avaliacao = await _avaliacaoService.AddAvaliacao(model);
                if (avaliacao == null) {
                   return NoContent(); 
                }
                await _avaliacaoNotifier.NovaAvaliacao(avaliacao.SessaoId);
                return Ok(avaliacao);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao inserir avaliação. Erro: {ex.Message}");
            }
        }

    }

}
