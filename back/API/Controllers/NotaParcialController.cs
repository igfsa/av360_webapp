using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Application.Contracts;
using Application.DTOs;
using Persistence.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotaParcialController : ControllerBase
    {
        private readonly INotaParcialService _NotasParciaisService;

        public NotaParcialController(INotaParcialService NotasParciaisService)
        {
            _NotasParciaisService = NotasParciaisService;
        }

        [HttpGet("{id:int}", Name = "GetNotaParcialId")]
        [ActionName("GetId")]
        public async Task<ActionResult<NotaParcialDTO>> Get(int id) {
            try {
                var NotaParcial = await _NotasParciaisService.GetById(id);
                if (NotaParcial is null)
                {
                    return NotFound();
                }
                return Ok(NotaParcial);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seção. Erro: {ex.Message}");
        }}

        [HttpGet("{turmaId:int}", Name = "GetNotaParcialNFinalId")]
        [ActionName("GetNotaParcialNFinalId")]
        public async Task<ActionResult<IEnumerable<NotaParcialDTO>>> GetNotaParcialNFinalId(int notaFinalId) {
            try {
                var NotasParciais = await _NotasParciaisService.GetNotaParcialNFinalId(notaFinalId);
                if (NotasParciais is null)
                {
                    return NotFound();
                }
                return Ok(NotasParciais);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar buscar Seções. Erro: {ex.Message}");
        }}

        [HttpPost]
        [ActionName("Post")]
        public async Task<ActionResult<NotaParcialDTO>> Post(NotaParcialDTO model) {
            try {
                var NotaParcial = await _NotasParciaisService.Add(model);
                if (model == null) 
                {
                   return NoContent(); 
                }
                return Ok(NotaParcial);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar Seção. Erro: {ex.Message}");
        }}

        [HttpPut("{id:int}")]
        [ActionName("Put")]
        public async Task<ActionResult> Put(int id, NotaParcialDTO model)
        {
            try
            {
                var NotaParcial = await _NotasParciaisService.Update(id, model);
                if (NotaParcial == null)
                {
                   return NoContent(); 
                }
                return Ok(NotaParcial);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                $"Erro ao tentar atualizar Seções. Erro: {ex.Message}");
            }
        }

    }

}
