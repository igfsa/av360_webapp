using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;
using API.Notifier;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AvaliacaoController(
    IAvaliacaoService avaliacaoService,
    ISessaoNotifier sessaoNotifier) : ControllerBase
{
    private readonly IAvaliacaoService _avaliacaoService = avaliacaoService;
    private readonly ISessaoNotifier _sessaoNotifier = sessaoNotifier;

    [AllowAnonymous]
    [HttpGet(Name = "GetValidaSessaoChavePub")]
    [ActionName("GetValidaSessaoChavePub")]
    public async Task<ActionResult<AvaliacaoPublicaDTO>> GetValidaSessaoChavePub(string token) {
        var avaliacao = await _avaliacaoService.GetValidaSessaoChavePub(token);
        return Ok(avaliacao);
    }

    [AllowAnonymous]
    [HttpGet(Name = "GeraNovaAvaliacaoEnvio")]
    [ActionName("GeraNovaAvaliacaoEnvio")]
    public async Task<ActionResult<AvaliacaoEnvioDTO>> GeraNovaAvaliacaoEnvio(int sessaoId, int grupoId, int avaliadorId, string deviceHash) {
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

    [AllowAnonymous]
    [HttpPost("")]
    [ActionName("Post")]
    public async Task<ActionResult> Post(AvaliacaoEnvioDTO model)
    {
        var avaliacao = await _avaliacaoService.AddAvaliacao(model);
        
        await _sessaoNotifier.NovaAvaliacao(model.SessaoId);

        return Ok(avaliacao);
    }
}