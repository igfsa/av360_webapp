using Domain.Entities;

namespace Persistence.Contracts;

public interface IAlunoPersist
{
    Task<Aluno[]> GetAllAlunosAsync();
    Task<Aluno> GetAlunoIdAsync(int alunoId);
}