using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class AlunoPersist : IAlunoPersist
{
    private readonly APIContext _context;

    public AlunoPersist(APIContext context)
    {
        _context = context;
    }
    public async Task<Aluno[]> GetAllAlunosAsync()
    {
        IQueryable<Aluno> query = _context.Alunos;

        query = query.AsNoTracking()
                        .OrderBy(a => a.Nome);

        return await query.ToArrayAsync();
    }
    public async Task<Aluno> GetAlunoIdAsync(int alunoId)
    {
        IQueryable<Aluno> query = _context.Alunos;

        query = query.AsNoTracking().OrderBy(a => a.Id)
                        .Where(a => a.Id == alunoId);

        return await query.FirstOrDefaultAsync();
    }
}
