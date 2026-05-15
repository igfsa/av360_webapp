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
    [HttpGet("{sessaoId:int}")]
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
    [HttpGet("{turmaId:int}")]
    [ActionName("GetValidaInicioSessao")]
    public async Task<ActionResult<SessaoValidacaoDTO>> GetValidaInicioSessao(int turmaId)
    {
        var validacao = await _sessoesService.GetValidaInicioSessao(turmaId);

        return Ok(validacao);
    }

    [Authorize]
    [HttpGet("{sessaoId:int}")]
    [ActionName("GetFaltamAvaliarSessao")]
    public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetFaltamAvaliarSessao(int sessaoId)
    {
        var validacao = await _sessoesService.GetFaltamAvaliarSessao(sessaoId);

        return Ok(validacao);
    }

    [Authorize]
    [HttpPost]
    [ActionName("PostSessao")]
    public async Task<ActionResult<SessaoDTO>> Post(SessaoDTO model)
    {
        var Sessao = await _sessoesService.Add(model);

        await _turmaNotifier.SessaoTurmaCriada(Sessao.TurmaId);
        return Ok(Sessao);
    }

    [Authorize]
    [HttpPut("{sessaoId:int}")]
    [ActionName("PutEncerraSessao")]
    public async Task<ActionResult> PutEncerra(int sessaoId, SessaoDTO model)
    {
        var Sessao = await _sessoesService.EncerrarSessao(sessaoId, model);

        await _sessaoNotifier.SessaoFinalizada(sessaoId);
        return Ok(Sessao);
    }
}
