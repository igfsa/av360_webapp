using Domain.Entities;

namespace Persistence.Contracts;

public interface INotaFinalPersist
{
    Task<NotaFinal[]> GetNotasFinalSessaoIdAsync(int sessaoId);
}