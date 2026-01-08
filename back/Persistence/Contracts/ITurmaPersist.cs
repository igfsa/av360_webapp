using Domain.Entities;

namespace Persistence.Contracts;

public interface ITurmaPersist
{
    Task<Turma[]> GetAllTurmasAsync();
    Task<Turma> GetTurmaIdAsync(int turmaId);
}