using Domain.Entities;

namespace Persistence.Contracts;

public interface IGrupoPersist
{
    Task<Grupo[]> GetAllGruposAsync();
    Task<Grupo> GetGrupoIdAsync(int grupoId);
    Task<Grupo[]> GetGruposTurmaIdAsync(int grupoId);
}