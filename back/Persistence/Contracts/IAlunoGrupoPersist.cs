using Domain.Entities;

namespace Persistence.Contracts;

public interface IAlunoGrupoPersist
{
    Task<Aluno[]> GetAlunosGrupoId(int grupoId);
    Task<AlunoGrupo[]> GetAlunosGrupoTurmaId(int turmaId);
    Task<Aluno> GetExisteAlunoGrupo(int grupoId, int alunoId);
}