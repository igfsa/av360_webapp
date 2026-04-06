using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class NotaParcialPersist(APIContext context) : INotaParcialPersist
{
    private readonly APIContext _context = context;

    public async Task<NotaParcial?> GetById(int notaParcialId)
    {
        return await _context.NotasParciais
            .Include(np => np.Avaliado)
            .Include(np => np.Avaliado)
            .Include(np => np.Avaliado)
            .FirstOrDefaultAsync(np => np.Id == notaParcialId);
    }
    public async Task<NotaParcial[]> GetNotaParcialNFinalIdAsync(int notaFinalId)
    {
        return await _context.NotasParciais
            .Where(np => np.NotaFinalId == notaFinalId)
            .OrderByDescending(np => np.Id)
            .ToArrayAsync();
    }
    public async Task<NotaParcial[]> GetNotaParcialSessaoIdAsync(int sessaoId)
    {
        return await _context.NotasParciais
            .Where(np => np.NotaFinal.SessaoId == sessaoId)
            .OrderByDescending(np => np.Id)
            .ToArrayAsync();
    }
    
}
