using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;
using API.Helpers;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SessaoController(ISessaoService sessoesService,
                    IExportService exportService,
                    ITurmaNotifier turmaNotifier,
                    ISessaoNotifier sessaoNotifier) : ControllerBase
{
    private readonly ISessaoService _sessoesService = sessoesService;
    private readonly IExportService _exportService = exportService;
    private readonly ITurmaNotifier _turmaNotifier = turmaNotifier;
    private readonly ISessaoNotifier _sessaoNotifier = sessaoNotifier;




    [Authorize]
    [HttpGet("{id:int}")]
    [ActionName("GetSessaoId")]
    public async Task<ActionResult<SessaoDTO>> Get(int id)
    {
        var Sessao = await _sessoesService.GetSessaoById(id);
        return Ok(Sessao);
    }

    [Authorize]
    [HttpGet("{turmaId:int}")]
    [ActionName("GetSessaoAtivaTurma")]
    public async Task<ActionResult<SessaoDTO>> GetSessaoAtivaTurmaIdAsync(int turmaId)
    {
        var sessao = await _sessoesService.GetSessaoAtivaTurmaIdAsync(turmaId);
        return Ok(sessao);
    }

    [Authorize]
    [HttpGet("{turmaId:int}")]
    [ActionName("GetSessoesTurmaId")]
    public async Task<ActionResult<IEnumerable<SessaoDTO>>> GetSessoesTurmaIdAsync(int turmaId)
    {
        var sessoes = await _sessoesService.GetSessoesTurmaIdAsync(turmaId);
        return Ok(sessoes);
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
    [HttpGet("{sessaoId:int}")]
    [ActionName("GetExportConsolidado")]
    public async Task<IActionResult> GetExportConsolidado(int sessaoId)
    {
        var dados = await _sessoesService.GetAvaliacaoConsolidada(sessaoId);

        var arquivo = await _exportService.ExportAvaliacaoConsolidada(dados);

        return File(
            arquivo,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "avaliacoes.xlsx"
        );
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostSessao")]
    public async Task<ActionResult<SessaoDTO>> Post(SessaoDTO model)
    {
        var Sessao = await _sessoesService.Add(model);

        await _turmaNotifier.TurmaAtualizada(Sessao.TurmaId);
        return Ok(Sessao);
    }

    [Authorize]
    [HttpPut("{sessaoId:int}")]
    [ActionName("PutEncerraSessao")]
    public async Task<ActionResult> PutEncerra(int sessaoId, SessaoDTO model)
    {
        var Sessao = await _sessoesService.EncerrarSessao(sessaoId, model);

        await _turmaNotifier.TurmaAtualizada(Sessao.TurmaId);
        await _sessaoNotifier.SessaoFinalizada(sessaoId);
        return Ok(Sessao);
    }
}
