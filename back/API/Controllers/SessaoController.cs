using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SessaoController : ControllerBase
    {
        private readonly ISessaoService _SessoesService;
        private readonly ISessaoNotifier _sessaoNotifier;
        private readonly ITurmaNotifier _turmaNotifier;
        public SessaoController(ISessaoService SessoesService,
                            ISessaoNotifier sessaoNotifier,
                            ITurmaNotifier turmaNotifier)
        {
            _SessoesService = SessoesService;
            _sessaoNotifier = sessaoNotifier;
            _turmaNotifier = turmaNotifier;
        }

        [HttpGet]
        [ActionName("GetAllSessoes")]
        public async Task<ActionResult<IEnumerable<SessaoDTO>>> Get() {
            try {
                var Sessoes = await _SessoesService.GetSessoes();
                if (Sessoes is null) {
                    return NotFound();
                }
                return Ok(Sessoes);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seções. Erro: {ex.Message}");
        }}

        [HttpGet("{id:int}", Name = "GetSessaoId")]
        [ActionName("GetId")]
        public async Task<ActionResult<SessaoDTO>> Get(int id) {
            try {
                var Sessao = await _SessoesService.GetSessaoById(id);
                if (Sessao is null) {
                    return NotFound();
                }
                return Ok(Sessao);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seção. Erro: {ex.Message}");
        }}

        [HttpGet("{turmaId:int}", Name = "GetSessoesTurma")]
        [ActionName("GetSessoesTurma")]
        public async Task<ActionResult<IEnumerable<SessaoDTO>>> GetSessoesTurma(int turmaId) {
            try {
                var Sessoes = await _SessoesService.GetSessoesTurma(turmaId);
                if (Sessoes is null) {
                    return NotFound();
                }
                return Ok(Sessoes);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seções. Erro: {ex.Message}");
        }}

        [HttpGet("{turmaId:int}", Name = "GetSessaoAtivaTurma")]
        [ActionName("GetSessaoAtivaTurma")]
        public async Task<ActionResult<SessaoDTO>> GetSessaoAtivaTurmaIdAsync(int turmaId) {
            try {
                var sessao = await _SessoesService.GetSessaoAtivaTurmaIdAsync(turmaId);
                return Ok(sessao);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Sessão. Erro: {ex.Message}");
        }}

        [HttpGet(Name = "GetSessaoChavePub")]
        [ActionName("GetSessaoChavePub")]
        public async Task<ActionResult<SessaoDTO>> GetSessaoChavePub(string token) {
            try{
                var sessao = await _SessoesService.GetSessaoChavePub(token);
                if (sessao == null)
                    return NotFound("Sessão inválida ou encerrada");
                return Ok(sessao);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Sessão. Erro: {ex.Message}");
        }}
        
        [HttpGet("{sessaoId}/qrcode")]
        public async Task<IActionResult> GetQrCode(int sessaoId)
        {
            var sessao = await _SessoesService.GetSessaoById(sessaoId);
            if (sessao == null) 
                return NotFound();

            var qrBytes = GeradorQrCode.GenerateQrCode($"https://seusite.com/avaliacao/publica/{sessao.TokenPublico}");

            return File(qrBytes, "image/png");
        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult<SessaoDTO>> Post(SessaoDTO model) {
            try {
                var Sessao = await _SessoesService.Add(model);
                if (model == null)  {
                   return NoContent(); 
                }
                await _turmaNotifier.TurmaAtualizadaAsync(Sessao.TurmaId);
                return Ok(Sessao);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Seção. Erro: {ex.Message}");
        }}

        [HttpPut("{id:int}")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(int id, SessaoDTO model) {
            try {
                var Sessao = await _SessoesService.Update(id, model);
                if (Sessao == null) {
                   return NoContent(); 
                }
                return Ok(Sessao);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar Seções. Erro: {ex.Message}");
        }}

        [HttpPut("{id:int}")]
        [ActionName("PutEncerra")]
        public async Task<ActionResult> PutEncerra(int id, SessaoDTO model) {
            try {
                var Sessao = await _SessoesService.EncerrarSessao(id, model);
                if (Sessao == null) {
                   return NoContent(); 
                }
                await _turmaNotifier.TurmaAtualizadaAsync(Sessao.TurmaId);
                return Ok(Sessao);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar Seções. Erro: {ex.Message}");
        }}
    }
}
