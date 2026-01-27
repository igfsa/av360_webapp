using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class GrupoPersist : IGrupoPersist
{
    private readonly APIContext _context;

    public GrupoPersist(APIContext context){
        _context = context;
    }
    public async Task<Grupo[]> GetAllGruposAsync(){
        return await _context.Grupos.AsNoTracking().OrderBy(g => g.Nome).ToArrayAsync();
    }
    public async Task<Grupo?> GetGrupoIdAsync(int grupoId){
        return await _context.Grupos.AsNoTracking().FirstOrDefaultAsync(g => g.Id == grupoId);
    }
    public async Task<Grupo[]> GetGruposTurmaIdAsync(int turmaId) {
        return await _context.Grupos
            .AsNoTracking()
            .Where(g => g.TurmaId == turmaId)
            .OrderBy(g => g.Nome)
            .ToArrayAsync();
    }
}
