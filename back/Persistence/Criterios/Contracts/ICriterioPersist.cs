using Domain.Entities;

namespace Persistence.Contracts;

public interface ICriterioPersist
{
    Task<Criterio[]> GetAllCriteriosAsync();
    Task<Criterio[]> GetCriteriosProfessorAsync(int professorId);
    Task<Criterio?> GetCriterioIdAsync(int alunoId);
    Task<Criterio[]> GetCriteriosPadrao();
    Task<Criterio[]> GetCriteriosSugestaoAsync(int professorId);
}