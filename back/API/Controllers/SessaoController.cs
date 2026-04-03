using Microsoft.AspNetCore.Mvc;

using Application.Contracts;
using Application.DTOs;
using API.Helpers;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SessaoController(ISessaoService SessoesService,
                    ITurmaNotifier turmaNotifier) : ControllerBase
{
    private readonly ISessaoService _SessoesService = SessoesService;
    private readonly ITurmaNotifier _turmaNotifier = turmaNotifier;

    [HttpGet]
    [ActionName("GetAllSessoes")]
    public async Task<ActionResult<IEnumerable<SessaoDTO>>> Get()
    {
        var Sessoes = await _SessoesService.GetSessoes();
        return Ok(Sessoes);
    }

    [HttpGet("{id:int}", Name = "GetSessaoId")]
    [ActionName("GetId")]
    public async Task<ActionResult<SessaoDTO>> Get(int id)
    {
        var Sessao = await _SessoesService.GetSessaoById(id);
        return Ok(Sessao);
    }

    [HttpGet("{turmaId:int}", Name = "GetSessoesTurma")]
    [ActionName("GetSessoesTurma")]
    public async Task<ActionResult<IEnumerable<SessaoDTO>>> GetSessoesTurma(int turmaId)
    {
        var Sessoes = await _SessoesService.GetSessoesTurma(turmaId);
        return Ok(Sessoes);
    }

    [HttpGet("{turmaId:int}", Name = "GetSessaoAtivaTurma")]
    [ActionName("GetSessaoAtivaTurma")]
    public async Task<ActionResult<SessaoDTO>> GetSessaoAtivaTurmaIdAsync(int turmaId)
    {
        var sessao = await _SessoesService.GetSessaoAtivaTurmaIdAsync(turmaId);
        return Ok(sessao);
    }

    [HttpGet("{sessaoId}/qrcode")]
    public async Task<IActionResult> GetQrCode(int sessaoId)
    {
        var sessao = await _SessoesService.GetSessaoById(sessaoId);

        var qrBytes = GeradorQrCode.GenerateQrCode($"http://localhost:4200/avaliacao/publica/{sessao.TokenPublico}");

        return File(qrBytes, "image/png");
    }

    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<SessaoDTO>> Post(SessaoDTO model)
    {
        var Sessao = await _SessoesService.Add(model);

        await _turmaNotifier.TurmaAtualizadaAsync(Sessao.TurmaId);
        return Ok(Sessao);
    }

    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, SessaoDTO model)
    {
        var Sessao = await _SessoesService.Update(id, model);
        return Ok(Sessao);
    }

    [HttpPut("{id:int}")]
    [ActionName("PutEncerra")]
    public async Task<ActionResult> PutEncerra(int id, SessaoDTO model)
    {
        var Sessao = await _SessoesService.EncerrarSessao(id, model);

        await _turmaNotifier.TurmaAtualizadaAsync(Sessao.TurmaId);
        return Ok(Sessao);
    }
}
