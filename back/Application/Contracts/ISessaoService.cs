using Application.DTOs;

namespace Application.Contracts;

public interface ISessaoService
{
    Task<IEnumerable<SessaoDTO>> GetSessoes();
    Task <SessaoDTO> GetSessaoById(int Id);
    Task<IEnumerable<SessaoDTO>> GetSessoesTurma(int turmaId);
    Task<SessaoDTO> GetSessaoAtivaTurmaIdAsync(int turmaId);
    Task<SessaoDTO> GetSessaoChavePub(string token);
    Task<SessaoDTO> Add(SessaoDTO model);
    Task<SessaoDTO> Update(int SessaoId, SessaoDTO model);
    Task<SessaoDTO> EncerrarSessao(int SessaoId, SessaoDTO model);
}
