using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class SessaoPersist : ISessaoPersist
{
    private readonly APIContext _context;

    public SessaoPersist(APIContext context){
        _context = context;
    }
    public async Task<Sessao[]> GetAllSessoesAsync(){
        return await _context.Sessoes.AsNoTracking().OrderByDescending(a => a.DataInicio).ToArrayAsync();
    }
    public async Task<Sessao?> GetSessaoIdAsync(int SessaoId){
        return await _context.Sessoes.AsNoTracking().FirstOrDefaultAsync(s => s.Id == SessaoId);
    }
    public async Task<Sessao[]> GetSessoesTurmaIdAsync(int turmaId) {
        return await _context.Sessoes
            .AsNoTracking()
            .Where(s => s.TurmaId == turmaId)
            .OrderByDescending(s => s.DataInicio)
            .ToArrayAsync();
    }
}
