using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class NotaFinalPersist(APIContext context) : INotaFinalPersist
{
    private readonly APIContext _context = context;

    public async Task<NotaFinal[]> GetNotasFinalSessaoIdAsync(int sessaoId)
    {
        return await _context.NotasFinais
            .AsNoTracking()
            .Where(nf => nf.SessaoId == sessaoId)
            .Include(nf => nf.NotasParcial)
            .ToArrayAsync();
    }
}
