using Domain.Entities;

namespace Persistence.Contracts;

public interface ICriterioPersist
{
    Task<Criterio[]> GetAllCriteriosAsync();
    Task<Criterio> GetCriterioIdAsync(int alunoId);
}