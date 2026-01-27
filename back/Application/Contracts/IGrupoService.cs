using Application.DTOs;

namespace Application.Contracts;

public interface IGrupoService
{
    Task<IEnumerable<GrupoDTO>> GetGrupos();
    Task <GrupoDTO> GetGrupoById(int Id);
    Task<IEnumerable<GrupoDTO>> GetGruposTurma(int turmaId);
    Task<GrupoDTO> Add(GrupoDTO model);
    Task<GrupoDTO> Update(int grupoId, GrupoDTO model);
    Task<GrupoDTO> AddAlunoGrupo(AlunoGrupoDTO model);
}
