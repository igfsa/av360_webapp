using Application.DTOs;

namespace Application.Contracts;

public interface IGrupoService
{
    Task<GrupoDTO> GetGrupoById(int Id);
    Task<IEnumerable<GrupoDTO>> GetGruposTurma(int turmaId);
    Task<IEnumerable<AlunoGrupoCheckboxDTO>> GetAlunoGrupoTurma(int turmaId, int grupoId);
    Task<GrupoDTO> Add(GrupoDTO model);
    Task<GrupoDTO> Update(int grupoId, GrupoDTO model);
    Task AtualizarGrupo(int turmaId, int grupoId, List<int> alunosSelecionados);
}
