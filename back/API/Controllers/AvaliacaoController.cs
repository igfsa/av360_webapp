using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;
using Application.Services;

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

        [HttpGet(Name = "GetValidaSessaoChavePub")]
        [ActionName("GetValidaSessaoChavePub")]
        public async Task<ActionResult<AvaliacaoPublicaDTO>> GetValidaSessaoChavePub(string token) {
            try{
                var avaliacao = await _avaliacaoService.GetValidaSessaoChavePub(token);
                if (avaliacao == null)
                    return NotFound("Sessão inválida ou encerrada");
                return Ok(avaliacao);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Sessão. Erro: {ex.Message}");
        }}

        [HttpGet(Name = "GeraNovaAvaliacaoEnvio")]
        [ActionName("GeraNovaAvaliacaoEnvio")]
        public async Task<ActionResult<AvaliacaoEnvioDTO>> GeraNovaAvaliacaoEnvio(int sessaoId, int grupoId, int avaliadorId, string deviceHash) {
            try{
                var avaliacao = new AvaliacaoEnvioDTO
                {
                    SessaoId = sessaoId,
                    GrupoId = grupoId,
                    AvaliadorId = avaliadorId,
                    DeviceHash = deviceHash
                }; 
                var avaliacaoRes = await _avaliacaoService.GeraNovaAvaliacaoEnvio(avaliacao);
                return Ok(avaliacaoRes);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Sessão. Erro: {ex.Message}");
        }}

        [HttpPost("")]
        [ActionName("Post")]
        public async Task<ActionResult> Post(AvaliacaoEnvioDTO model)
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
