using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class AlunoTurmaPersist(APIContext context) : IAlunoTurmaPersist
{
    private readonly APIContext _context = context;

    public async Task<Aluno[]> GetAlunosTurmaIdAsync(int turmaId)
    {
        var turma = await _context.Turmas
            .Include(t => t.Alunos)
            .FirstOrDefaultAsync(t => t.Id == turmaId);
        return [.. turma?.Alunos ?? []];
    }
    public async Task<Aluno?> GetExisteAlunoTurma(int turmaId, int alunoId)
    // Retorna um Aluno caso exista o AlunoTurma
    {
        var turma = await _context.Turmas
            .Include(a => a.Alunos)
            .FirstOrDefaultAsync(t => t.Id == turmaId);
        if (turma == null)
            return null;
        return turma.Alunos
            .FirstOrDefault(a => a.Id == alunoId);
    }
}
