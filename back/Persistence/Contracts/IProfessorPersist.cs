using Domain.Entities;

namespace Persistence.Contracts;

public interface IProfessorPersist
{
    Task<Professor[]> GetAllProfessores();
    Task<Professor?> GetProfessorUser(string userName);
}