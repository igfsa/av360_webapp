using Application.DTOs;
using Domain.Entities;

namespace Application.Contracts;

public interface ITurmaService
{
    Task<IEnumerable<TurmaDTO>> GetTurmas();
    Task<TurmaDTO> GetTurmaById(int Id);
    Task<IEnumerable<TurmaDTO>> GetTurmasAluno(int alunoId);
    Task<IEnumerable<TurmaDTO>> GetTurmasCriterio(int criterioId);
    Task<TurmaDTO> Add(TurmaDTO turma);
    Task<AlunoDTO> AddTurmaAluno(int alunoId, int turmaId);
    Task<CriterioDTO> AddTurmaCriterio(int criterioId, int turmaId);
    Task<TurmaDTO> Update(int turmaId, TurmaDTO turma);
}
