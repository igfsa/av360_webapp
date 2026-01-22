using Domain.Entities;

namespace Persistence.Contracts;

public interface ICriterioTurmaPersist
{
    Task<Criterio[]> GetCriteriosTurmaIdAsync(int turmaId);
    Task<Turma[]> GetTurmasCriterioIdAsync(int criterioId);
    Task<Criterio> GetValidaCriterioTurma(int turmaId, int criterioId);
}