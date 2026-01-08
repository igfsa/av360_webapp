using Domain.Entities;

namespace Persistence.Contracts;

public interface IAlunoTurmaPersist
{
    Task<Turma[]> GetAlunoTurmaIdAsync(int alunoId);
    Task<Aluno[]> GetTurmaAlunoIdAsync(int turmaId);
    Task<Aluno> GetValidaAlunoTurma(int turmaId, int alunoId);
}