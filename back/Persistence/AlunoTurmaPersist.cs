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

    public async Task<Turma[]> GetAlunoTurmaIdAsync(int alunoId)
    {
        IQueryable<Turma> query = _context.AlunoTurma
            .AsNoTracking()
            .Where(at => at.AlunoId == alunoId)
            .Select(at => at.Turma)
            .OrderBy(a => a.Cod);
        return await query.ToArrayAsync();
        
    }
    public async Task<Aluno[]> GetTurmaAlunoIdAsync(int turmaId)
    {
        IQueryable<Aluno> query = _context.AlunoTurma
            .AsNoTracking()
            .Where(at => at.TurmaId == turmaId)            
            .Select(at => at.Aluno)
            .OrderBy(a => a.Nome);
        return await query.ToArrayAsync();
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
