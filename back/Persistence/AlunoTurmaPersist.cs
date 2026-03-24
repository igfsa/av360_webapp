using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class AlunoTurmaPersist : IAlunoTurmaPersist
{
    private readonly APIContext _context;

    public AlunoTurmaPersist(APIContext context) {
        _context = context;
    }

    public async Task<Aluno[]> GetAlunosTurmaIdAsync(int turmaId) {
        var turma = await _context.Turmas
            .Include(t => t.Alunos)
            .FirstOrDefaultAsync(t => t.Id == turmaId)
                ?? null!;
        return turma.Alunos.ToArray();
    }
    public async Task<Turma[]> GetTurmasAlunoIdAsync(int alunoId) {
        var aluno = await _context.Alunos
            .Include(a => a.Turmas)
            .FirstOrDefaultAsync(a => a.Id == alunoId)
                ?? null!;
        return aluno.Turmas.ToArray();
    }
    public async Task<Aluno?> GetExisteAlunoTurma(int turmaId, int alunoId)
    // Retorna um Aluno caso exista o AlunoTurma
    {
        var turma = await _context.Turmas
            .Include(a => a.Alunos)
            .FirstOrDefaultAsync(t => t.Id == turmaId);
        return turma!.Alunos
            .FirstOrDefault(a => a.Id == alunoId);
    }
}
