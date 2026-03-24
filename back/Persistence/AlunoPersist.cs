using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class AlunoPersist : IAlunoPersist
{
    private readonly APIContext _context;

    public AlunoPersist(APIContext context){
        _context = context;
    }
    public async Task<Aluno[]> GetAllAlunosAsync(){
        return await _context.Alunos
            .Include(a => a.Turmas)
            .OrderBy(a => a.Nome)
            .ToArrayAsync();
    }
    public async Task<Aluno?> GetAlunoIdAsync(int alunoId){
        return await _context.Alunos
            .Include(a => a.Turmas)
            .FirstOrDefaultAsync(a => a.Id == alunoId);
    }
}
