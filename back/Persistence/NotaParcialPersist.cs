using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class NotaParcialPersist(APIContext context) : INotaParcialPersist
{
    private readonly APIContext _context = context;

    public async Task<NotaParcial[]> GetNotaParcialSessaoIdAsync(int sessaoId)
    {
        return await _context.NotasParciais
            .Where(np => np.NotaFinal.SessaoId == sessaoId)
            .OrderByDescending(np => np.Id)
            .ToArrayAsync();
    }
    
}
