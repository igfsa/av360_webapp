using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class AlunoTurmaPersist : IAlunoTurmaPersist
{
    private readonly APIContext _context;

    public AlunoTurmaPersist(APIContext context)
    {
        _context = context;
    }

    public async Task<Aluno[]> GetAlunosTurmaIdAsync(int turmaId)
    {
        return await _context.Alunos
            .AsNoTracking()
            .Where(a => _context.AlunoTurma
                .Any(at => at.TurmaId == turmaId && at.AlunoId == a.Id))
            .OrderBy(a => a.Nome)
            .ToArrayAsync();
    }
    public async Task<Turma[]> GetTurmasAlunoIdAsync(int alunoId)
    {
        return await _context.Turmas
            .AsNoTracking()
            .Where(t => _context.AlunoTurma
                .Any(at => at.AlunoId == alunoId && at.TurmaId == t.Id))
            .OrderBy(t => t.Cod)
            .ToArrayAsync();
    }
    public async Task<Aluno> GetValidaAlunoTurma(int turmaId, int alunoId)
    {
        if (await _context.AlunoTurma.AnyAsync(at => at.TurmaId == turmaId && at.AlunoId == alunoId))
        {
            return null;
        }
        else 
        {
            IQueryable<Aluno> query = _context.Alunos;

            query = query.AsNoTracking().OrderBy(a => a.Id)
                            .Where(a => a.Id == alunoId);
            return await query.FirstOrDefaultAsync();
        }
    }
}
