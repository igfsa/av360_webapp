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
    public async Task<Turma[]> GetTurmasCriterioIdAsync(int criterioId)
    {
        return await _context.Turmas
            .AsNoTracking()
            .Where(t => t.Criterios
                .Any(c => c.Id == criterioId))
            .OrderBy(t => t.Cod)
            .ToArrayAsync();
    }
    public async Task<Criterio?> GetExisteCriterioTurma(int turmaId, int criterioId)
    {
        var turma = await _context.Turmas
            .FirstOrDefaultAsync(t => t.Id == turmaId);
        return turma!.Criterios
            .FirstOrDefault(c => c.Id == criterioId);
    }
}
