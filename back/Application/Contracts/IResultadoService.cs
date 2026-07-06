using Application.DTOs;

namespace Application.Contracts;

public interface IResultadoService
{
    Task<ResultadoSessaoReportDTO> RecalcularResultadoSessao(int sessaoId);
}