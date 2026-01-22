using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class CriterioTurmaPersist : ICriterioTurmaPersist
{
    private readonly APIContext _context;

    public CriterioTurmaPersist(APIContext context)
    {
        _context = context;
    }

    public async Task<Criterio[]> GetCriteriosTurmaIdAsync(int turmaId)
    {
        return await _context.Criterios
            .AsNoTracking()
            .Where(c => _context.CriterioTurma
                .Any(ct => ct.TurmaId == turmaId && ct.CriterioId == c.Id))
            .OrderBy(c => c.Nome)
            .ToArrayAsync();
    }
    public async Task<Turma[]> GetTurmasCriterioIdAsync(int criterioId)
    {
        return await _context.Turmas
            .AsNoTracking()
            .Where(t => _context.CriterioTurma
                .Any(ct => ct.CriterioId == criterioId && ct.TurmaId == t.Id))
            .OrderBy(t => t.Cod)
            .ToArrayAsync();
    }
    public async Task<Criterio> GetValidaCriterioTurma(int turmaId, int criterioId)
    {
        if (await _context.CriterioTurma.AnyAsync(ct => ct.TurmaId == turmaId && ct.CriterioId == criterioId))
        {
            return null;
        }
        else 
        {
            IQueryable<Criterio> query = _context.Criterios;

            query = query.AsNoTracking().OrderBy(c => c.Id)
                            .Where(c => c.Id == criterioId);
            return await query.FirstOrDefaultAsync();
        }
    }
}
