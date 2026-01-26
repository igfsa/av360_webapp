using Domain.Entities;

namespace Persistence.Contracts;

public interface IAlunoTurmaPersist
{
    Task<Aluno[]> GetAlunosTurmaIdAsync(int turmaId);
    Task<Turma[]> GetTurmasAlunoIdAsync(int alunoId);
    Task<Aluno> GetExisteAlunoTurma(int turmaId, int alunoId);
}