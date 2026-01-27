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
        return await _context.Alunos.AsNoTracking().OrderBy(a => a.Nome).ToArrayAsync();
    }
    public async Task<Aluno?> GetAlunoIdAsync(int alunoId){
        return await _context.Alunos.AsNoTracking().OrderBy(a => a.Id).Where(a => a.Id == alunoId).FirstOrDefaultAsync();
    }
}
