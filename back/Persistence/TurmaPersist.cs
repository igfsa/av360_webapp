using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class TurmaPersist : ITurmaPersist
{
    private readonly APIContext _context;

    public TurmaPersist(APIContext context){
        _context = context;
    }
    public async Task<Turma[]> GetAllTurmasAsync(){
        return await  _context.Turmas.AsNoTracking().OrderBy(a => a.Cod).ToArrayAsync();;
    }
    public async Task<Turma?> GetTurmaIdAsync(int TurmaId){
        return await _context.Turmas.AsNoTracking().Where(a => a.Id == TurmaId).FirstOrDefaultAsync();
    }
    public async Task<Turma?> GetTurmaGrupoIdAsync(int grupoId){
        var grupo = await _context.Grupos.AsNoTracking().Where(g => g.Id == grupoId).FirstOrDefaultAsync();
        if (grupo == null)
            return null;
        return await _context.Turmas.AsNoTracking().Where(t => t.Id == grupo.TurmaId).FirstOrDefaultAsync();
    }
}
