using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class TurmaPersist(APIContext context) : ITurmaPersist
{
    private readonly APIContext _context = context;

    public async Task<Turma[]> GetAllTurmasAsync()
    {
        return await _context.Turmas
            .AsNoTracking()
            .OrderBy(a => a.Cod)
            .ToArrayAsync();
    }
    public async Task<Turma?> GetTurmaIdAsync(int TurmaId)
    {
        return await _context.Turmas
            .Include(t => t.Alunos)
            .Include(t => t.Criterios)
            .Include(t => t.Grupos)
            .FirstOrDefaultAsync(t => t.Id == TurmaId);
    }
    public async Task<Turma?> GetTurmaGrupoIdAsync(int grupoId)
    {
        var grupo = await _context.Grupos
            .FirstOrDefaultAsync(g => g.Id == grupoId)
                ?? null!;
        return grupo.Turma;
    }
}
