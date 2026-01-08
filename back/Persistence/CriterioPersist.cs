using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class CriterioPersist : ICriterioPersist
{
    private readonly APIContext _context;

    public CriterioPersist(APIContext context)
    {
        _context = context;
    }
    public async Task<Criterio[]> GetAllCriteriosAsync()
    {
        IQueryable<Criterio> query = _context.Criterios;

        query = query.AsNoTracking()
                        .OrderBy(a => a.Nome);

        return await query.ToArrayAsync();
    }
    public async Task<Criterio> GetCriterioIdAsync(int criterioId)
    {
        IQueryable<Criterio> query = _context.Criterios;

        query = query.AsNoTracking().OrderBy(a => a.Id)
                        .Where(a => a.Id == criterioId);

        return await query.FirstOrDefaultAsync();
    }
    public async Task<Criterio[]> GetCriteriosTurmaAsync(int turmaId)
    {
        IQueryable<Criterio> query = _context.Criterios;

        query = query.AsNoTracking()
                        .Where(c => c.TurmaId == turmaId)
                        .OrderBy(a => a.Nome);

        return await query.ToArrayAsync();
    }
}
