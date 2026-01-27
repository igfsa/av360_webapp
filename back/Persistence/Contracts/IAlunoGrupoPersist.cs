using Domain.Entities;

namespace Persistence.Contracts;

public interface IAlunoGrupoPersist
{
    Task<Aluno[]> GetAlunosGrupoId(int grupoId);
}