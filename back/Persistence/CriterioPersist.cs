using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class CriterioPersist : ICriterioPersist
{
    private readonly APIContext _context;

    public CriterioPersist(APIContext context) {
        _context = context;
    }
    public async Task<Criterio[]> GetAllCriteriosAsync() {
        return await _context.Criterios.AsNoTracking().OrderBy(a => a.Nome).ToArrayAsync();
    }
    public async Task<Criterio?> GetCriterioIdAsync(int criterioId){
        return await _context.Criterios.AsNoTracking().OrderBy(a => a.Id).Where(a => a.Id == criterioId).FirstOrDefaultAsync();
    }
}
