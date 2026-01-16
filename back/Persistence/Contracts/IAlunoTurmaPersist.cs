using Domain.Entities;

namespace Persistence.Contracts;

public interface IAlunoTurmaPersist
{
    Task<Aluno[]> GetAlunosTurmaIdAsync(int turmaId);
    Task<Turma[]> GetTurmasAlunoIdAsync(int alunoId);
    Task<Aluno> GetValidaAlunoTurma(int turmaId, int alunoId);
}