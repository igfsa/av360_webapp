using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;
using API.Helpers;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ResultadoController(
        IResultadoService resultadoService,
        IExportService exportService
        ) : ControllerBase
{
    private readonly IResultadoService _resultadoService = resultadoService;
    private readonly IExportService _exportService = exportService;

    [Authorize]
    [HttpPut("{sessaoId:int}")]
    [ActionName("RecalcularResultadoSessao")]
    public async Task<ActionResult> RecalcularResultadoSessao(int sessaoId)
    {
        var dados = await _resultadoService.RecalcularResultadoSessao(sessaoId);

        if (dados.TotalGeralErros > 0)
        {
            var arquivo = await _exportService.ExportResultadoSessao(dados);

            return File(
                arquivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "resultado.xlsx"
            );
        } else {
            return NoContent();
        }
    }
}