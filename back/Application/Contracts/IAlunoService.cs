using Application.DTOs;

namespace Application.Contracts;

public interface IAlunoService
{
    Task<IEnumerable<AlunoDTO>> GetAlunos();
    Task<AlunoDTO> GetAlunoById(int Id);
    Task<AlunoDTO> GetAlunoByNomeIdGrupo(string nome, int grupoId);
    Task<IEnumerable<AlunoDTO>> GetAlunosTurma(int turmaId);
    Task<IEnumerable<AlunoDTO>> GetAlunosGrupo(int grupoId);
    Task<IEnumerable<AlunoGrupoNomeDTO>> GetAlunoGrupoNome(int turmaId);
    Task<AlunoDTO> Add(AlunoDTO model);
    Task<AlunoDTO> Update(int alunoId, AlunoDTO model);
}
