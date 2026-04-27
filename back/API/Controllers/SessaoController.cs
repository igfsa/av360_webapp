using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;
using API.Helpers;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SessaoController(ISessaoService sessoesService,
                    ITurmaNotifier turmaNotifier) : ControllerBase
{
    private readonly ISessaoService _sessoesService = sessoesService;
    private readonly ITurmaNotifier _turmaNotifier = turmaNotifier;


    [Authorize]
    [HttpGet("{turmaId:int}", Name = "GetSessaoAtivaTurma")]
    [ActionName("GetSessaoAtivaTurma")]
    public async Task<ActionResult<SessaoDTO>> GetSessaoAtivaTurmaIdAsync(int turmaId)
    {
        var sessao = await _sessoesService.GetSessaoAtivaTurmaIdAsync(turmaId);
        return Ok(sessao);
    }

    [AllowAnonymous]
    [HttpGet("{sessaoId}")]
    [ActionName("GetQrCode")]
    public async Task<IActionResult> GetQrCode(int sessaoId)
    {
        var sessao = await _sessoesService.GetSessaoById(sessaoId);

        var qrBytes = GeradorQrCode.GenerateQrCode($"/avaliacao/publica/{sessao.TokenPublico}");

        return File(qrBytes, "image/png");
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostSessao")]
    public async Task<ActionResult<SessaoDTO>> Post(SessaoDTO model)
    {
        var Sessao = await _sessoesService.Add(model);

        await _turmaNotifier.TurmaAtualizadaAsync(Sessao.TurmaId);
        return Ok(Sessao);
    }

    [Authorize]
    [HttpPut("{sessaoId:int}")]
    [ActionName("PutEncerraSessao")]
    public async Task<ActionResult> PutEncerra(int sessaoId, SessaoDTO model)
    {
        var Sessao = await _sessoesService.EncerrarSessao(sessaoId, model);

        await _turmaNotifier.TurmaAtualizadaAsync(Sessao.TurmaId);
        return Ok(Sessao);
    }
}
