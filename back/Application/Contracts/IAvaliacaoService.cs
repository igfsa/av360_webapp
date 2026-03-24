using Application.DTOs;

namespace Application.Contracts;

public interface IAvaliacaoService
{
    Task<NotaFinalDTO> AddAvaliacao(AvaliacaoEnvioDTO model);
    Task<AvaliacaoPublicaDTO> GetValidaSessaoChavePub(string token);
    Task<AvaliacaoEnvioDTO> GeraNovaAvaliacaoEnvio(AvaliacaoEnvioDTO avaliacao) ;
}
