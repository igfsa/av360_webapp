using Application.DTOs;

namespace Application.Contracts;

public interface IAlunoService
{
    Task<AlunoDTO> GetAlunoByNomeIdGrupo(string nome, int grupoId);
    Task<IEnumerable<AlunoDTO>> GetAlunosTurma(int turmaId);
    Task<IEnumerable<AlunoDTO>> GetAlunosGrupo(int grupoId);
    Task<IEnumerable<AlunoGrupoNomeDTO>> GetAlunoGrupoNome(int turmaId);
    Task<AlunoDTO> AddAlunoTurma(int turmaId, AlunoDTO alunoDto);
}
