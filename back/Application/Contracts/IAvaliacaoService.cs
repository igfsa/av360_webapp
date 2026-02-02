using Application.DTOs;

namespace Application.Contracts;

public interface IAvaliacaoService
{
    Task<NotaFinalDTO> AddAvaliacao(AvaliacaoEnvioDTO model);
}
