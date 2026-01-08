using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class TurmaPersist : ITurmaPersist
{
    private readonly APIContext _context;

    public TurmaPersist(APIContext context)
    {
        _context = context;
    }
    public async Task<Turma[]> GetAllTurmasAsync()
    {
        IQueryable<Turma> query = _context.Turmas;

        query = query.AsNoTracking()
                        .OrderBy(a => a.Cod);

        return await query.ToArrayAsync();
    }
    public async Task<Turma> GetTurmaIdAsync(int TurmaId)
    {
        IQueryable<Turma> query = _context.Turmas;

        query = query.AsNoTracking().OrderBy(a => a.Id)
                        .Where(a => a.Id == TurmaId);

        return await query.FirstOrDefaultAsync();
    }
}
