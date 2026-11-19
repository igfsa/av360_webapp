using Domain.Entities;

namespace Persistence.Contracts;

public interface IGrupoPersist
{
    Task<Grupo?> GetGrupoIdAsync(int grupoId);
    Task<Grupo[]> GetGruposTurmaIdAsync(int turmaId);
}