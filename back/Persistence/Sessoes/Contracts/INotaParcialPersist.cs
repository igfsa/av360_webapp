using Domain.Entities;

namespace Persistence.Contracts;

public interface INotaParcialPersist
{
    Task<NotaParcial[]> GetNotaParcialSessaoIdAsync(int sessaoId);
}