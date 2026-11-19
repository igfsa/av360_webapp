using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class TurmaPersist(APIContext context) : ITurmaPersist
{
    private readonly APIContext _context = context;

    public async Task<Turma[]> GetAllTurmasAsync()
    {
        return await _context.Turmas
            .Include(t => t.Professor)
            .AsNoTracking()
            .OrderBy(a => a.Cod)
            .ToArrayAsync();
    }
    public async Task<Turma?> GetTurmaIdAsync(int TurmaId)
    {
        return await _context.Turmas
            .Include(t => t.Alunos)
            .Include(t => t.Criterios)
            .Include(t => t.Grupos)
            .Include(t => t.Professor)
            .FirstOrDefaultAsync(t => t.Id == TurmaId);
    }
    public async Task<Turma[]> GetTurmasProfessorIdAsync(int professorId)
    {
        var professor = await _context.Professores
            .Include(p => p.Turmas)
            .FirstOrDefaultAsync(p => p.Id == professorId);

        return [.. professor?.Turmas ?? []];
    }
}
