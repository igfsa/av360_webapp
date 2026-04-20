using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;
using System.Reflection.Metadata.Ecma335;

namespace Persistence;

public class GrupoPersist(APIContext context) : IGrupoPersist
{
    private readonly APIContext _context = context;

    public async Task<Grupo[]> GetAllGruposAsync()
    {
        return await _context.Grupos
            .AsNoTracking()
            .OrderBy(g => g.Nome)
            .ToArrayAsync();
    }
    public async Task<Grupo?> GetGrupoIdAsync(int grupoId)
    {
        return await _context.Grupos
            .Include(g => g.Turma)
            .FirstOrDefaultAsync(g => g.Id == grupoId);
    }
    public async Task<Grupo[]> GetGruposTurmaIdAsync(int turmaId)
    {
        var turma = await _context.Turmas
            .Include(t => t.Grupos)
            .FirstOrDefaultAsync(t => t.Id == turmaId);
        if (turma == null)
            return [];

        return [.. turma.Grupos];
    }
}
