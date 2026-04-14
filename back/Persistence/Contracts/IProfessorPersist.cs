using Domain.Entities;

namespace Persistence.Contracts;

public interface IProfessorPersist
{
    Task<Professor?> GetProfessorUser(string userName);
}