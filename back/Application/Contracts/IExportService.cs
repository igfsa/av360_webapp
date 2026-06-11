using Application.DTOs;

namespace Application.Contracts;

public interface IExportService 
{
    Task<byte[]> ExportAvaliacaoConsolidada(List<AvaliacaoConsolidadaExportDTO> alunosNotas);
    Task<byte[]> ExportResultadoSessao(ResultadoSessaoReportDTO report);
}
