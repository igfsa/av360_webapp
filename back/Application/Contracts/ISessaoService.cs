using Application.DTOs;

namespace Application.Contracts;

public interface ISessaoService
{
    Task<SessaoDTO> GetSessaoById(int Id);
    Task<SessaoDTO> GetSessaoAtivaTurmaIdAsync(int turmaId);
    Task<IEnumerable<SessaoDTO>> GetSessoesTurmaIdAsync(int turmaId);
    Task<SessaoDTO> Add(SessaoDTO model);
    Task<SessaoDTO> EncerrarSessao(int SessaoId, SessaoDTO model);
    Task<List<AvaliacaoConsolidadaExportDTO>> GetAvaliacaoConsolidada(int sessaoId);
}
