using Microsoft.AspNetCore.Mvc;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotaParcialController(INotaParcialService NotasParciaisService) : ControllerBase
{
    private readonly INotaParcialService _NotasParciaisService = NotasParciaisService;

    [HttpGet("{id:int}", Name = "GetNotaParcialId")]
    [ActionName("GetId")]
    public async Task<ActionResult<NotaParcialDTO>> Get(int id)
    {
        var NotaParcial = await _NotasParciaisService.GetById(id);
        return Ok(NotaParcial);
    }

    [HttpGet("{turmaId:int}", Name = "GetNotaParcialNFinalId")]
    [ActionName("GetNotaParcialNFinalId")]
    public async Task<ActionResult<IEnumerable<NotaParcialDTO>>> GetNotaParcialNFinalId(int notaFinalId)
    {
        var NotasParciais = await _NotasParciaisService.GetNotaParcialNFinalId(notaFinalId);
        return Ok(NotasParciais);
    }

    [HttpPost]
    [ActionName("Post")]
    public async Task<ActionResult<NotaParcialDTO>> Post(NotaParcialDTO model)
    {
        var NotaParcial = await _NotasParciaisService.Add(model);
        return Ok(NotaParcial);
    }

    [HttpPut("{id:int}")]
    [ActionName("Put")]
    public async Task<ActionResult> Put(int id, NotaParcialDTO model)
    {
        var NotaParcial = await _NotasParciaisService.Update(id, model);
        return Ok(NotaParcial);
    }
}
