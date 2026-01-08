using Application.DTOs;

namespace Application.Contracts;

public interface IAlunoService
{
    Task<IEnumerable<AlunoDTO>> GetAlunos();
    Task <AlunoDTO> GetAlunoById(int Id);
    Task<IEnumerable<TurmaDTO>> GetAlunoTurma(int alunoId);
    Task<AlunoDTO> Add(AlunoDTO model);
    Task<AlunoDTO> Update(int alunoId, AlunoDTO model);
}
