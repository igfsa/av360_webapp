using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class NotaFinalPersist : INotaFinalPersist
{
    private readonly APIContext _context;

    public NotaFinalPersist(APIContext context){
        _context = context;
    }
    public async Task<NotaFinal?> GetNotaFinalIdAsync(int notaFinalId){
        return await _context.NotasFinais.AsNoTracking().FirstOrDefaultAsync(nf => nf.Id == notaFinalId);
    }
    public async Task<NotaFinal?> GetNotaFinalAlunoSessaoIdAsync(int alunoId, int sessaoId) {
        return await _context.NotasFinais.AsNoTracking().FirstOrDefaultAsync(nf => nf.AvaliadorId == alunoId && nf.SessaoId == sessaoId);
    }
    public async Task<NotaFinal[]> GetNotaFinalGrupoSessaoIdAsync(int grupoId, int sessaoId) {
        return await _context.NotasFinais
            .AsNoTracking()
            .Where(nf => nf.SessaoId == sessaoId && nf.GrupoId == grupoId)
            .OrderByDescending(nf => nf.DataEnvio)
            .ToArrayAsync();
    }
    public async Task<NotaFinal?> GetNotaFinalHashAsync(string deviceHash, int sessaoId) {
        return await _context.NotasFinais.AsNoTracking().FirstOrDefaultAsync(nf => nf.DeviceHash == deviceHash && nf.SessaoId == sessaoId);
    }
}
