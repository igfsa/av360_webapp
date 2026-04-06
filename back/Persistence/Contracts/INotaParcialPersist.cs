using Domain.Entities;

namespace Persistence.Contracts;

public interface INotaParcialPersist
{
    Task<NotaParcial?> GetById(int id);
    Task<NotaParcial[]> GetNotaParcialNFinalIdAsync(int notaFinalId);
    Task<NotaParcial[]> GetNotaParcialSessaoIdAsync(int sessaoId);
}