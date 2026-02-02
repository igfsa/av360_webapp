using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class NotaParcialPersist : INotaParcialPersist
{
    private readonly APIContext _context;

    public NotaParcialPersist(APIContext context){
        _context = context;
    }
    public async Task<NotaParcial?> GetById(int notaParcialId){
        return await _context.NotasParciais.AsNoTracking().FirstOrDefaultAsync(np => np.Id == notaParcialId);
    }
    public async Task<NotaParcial[]> GetNotaParcialNFinalIdAsync(int notaFinalId) {
        return await _context.NotasParciais
            .AsNoTracking()
            .Where(np => np.NotaFinalId == notaFinalId)
            .OrderByDescending(np => np.Id)
            .ToArrayAsync();
    }
}
