using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class CriterioTurmaPersist(APIContext context) : ICriterioTurmaPersist
{
    private readonly APIContext _context = context;

    public async Task<Criterio[]> GetCriteriosTurmaIdAsync(int turmaId)
    {
        return await _context.Criterios
                .AsNoTracking()
                .Where(c => c.Turmas
                    .Any(t => t.Id == turmaId))
                .OrderBy(c => c.Nome)
                .ToArrayAsync();
    }
}
