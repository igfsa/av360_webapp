using Domain.Entities;

namespace Persistence.Contracts;

public interface ICriterioTurmaPersist
{
    Task<Criterio[]> GetCriteriosTurmaIdAsync(int turmaId);
}