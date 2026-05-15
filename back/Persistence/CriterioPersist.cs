using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class CriterioPersist(APIContext context) : ICriterioPersist
{
    private readonly APIContext _context = context;

    public async Task<Criterio[]> GetAllCriteriosAsync()
    {
        return await _context.Criterios
            .OrderBy(a => a.Nome)
            .ToArrayAsync();
    }
    public async Task<Criterio?> GetCriterioIdAsync(int criterioId)
    {
        return await _context.Criterios
            .FirstOrDefaultAsync(c => c.Id == criterioId);
    }
    public async Task<Criterio[]> GetCriteriosPadrao()
    {
        return await _context.Criterios.Where(c => c.Id >= 1 && c.Id <= 9).ToArrayAsync();
    }
}
