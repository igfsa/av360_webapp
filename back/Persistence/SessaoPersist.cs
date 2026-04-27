using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class SessaoPersist(APIContext context) : ISessaoPersist
{
    private readonly APIContext _context = context;

    public async Task<Sessao?> GetSessaoIdAsync(int SessaoId)
    {
        return await _context.Sessoes
            .Include(s => s.Notasfinais)
            .Include(s => s.Turma)
            .FirstOrDefaultAsync(s => s.Id == SessaoId);
    }
    public async Task<Sessao?> GetSessaoAtivaTurmaIdAsync(int TurmaId)
    {
        return await _context.Sessoes
            .Include(s => s.Notasfinais)
            .FirstOrDefaultAsync(s => s.TurmaId == TurmaId && s.Ativo);
    }
    public async Task<Sessao?> GetValidaSessaoChavePubAsync(string token)
    {
        return await _context.Sessoes
            .FirstOrDefaultAsync(s => 
                s.TokenPublico == token
                && s.Ativo
                && s.DataFim == null
            );
    }
}
