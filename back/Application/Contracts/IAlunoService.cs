using Application.DTOs;

namespace Application.Contracts;

public interface IAlunoService
{
    Task<IEnumerable<AlunoDTO>> GetAlunos();
    Task <AlunoDTO> GetAlunoById(int Id);
    Task<IEnumerable<AlunoDTO>> GetAlunosTurma(int turmaId);
    Task<AlunoDTO> Add(AlunoDTO model);
    Task<AlunoDTO> Update(int alunoId, AlunoDTO model);
}
