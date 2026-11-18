using Application.DTOs;

namespace Application.Contracts;

public interface ISessaoService
{
    Task<SessaoDTO> GetSessaoById(int Id);
    Task<SessaoDTO> GetSessaoAtivaTurmaIdAsync(int turmaId);
    Task<IEnumerable<SessaoDTO>> GetSessoesTurmaIdAsync(int turmaId);
    Task<SessaoValidacaoDTO> GetValidaInicioSessao(int turmaId);
    Task<IEnumerable<AlunoDTO>> GetFaltamAvaliarSessao(int sessaoId);
    Task<SessaoDTO> Add(SessaoDTO model);
    Task<ResultadoSessaoReportDTO> EncerrarSessao(int SessaoId);
    Task<List<AvaliacaoConsolidadaExportDTO>> GetAvaliacaoConsolidada(int sessaoId);
}
