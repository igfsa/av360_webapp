using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class AlunoGrupoPersist : IAlunoGrupoPersist
{
    private readonly APIContext _context;

    public AlunoGrupoPersist(APIContext context) {
        _context = context;
    }

    public async Task<Aluno[]> GetAlunosGrupoId(int grupoId)
    {
        return await _context.Alunos
            .AsNoTracking()
            .Where(a => _context.AlunoGrupo
                .Any(ag => ag.GrupoId == grupoId && ag.AlunoId == a.Id))
            .OrderBy(a => a.Nome)
            .ToArrayAsync();
    }
    public async Task<Aluno?> GetExisteAlunoGrupo(int grupoId, int alunoId)
    // Retorna um Aluno caso exista o AlunoGrupo
    {
        if (await _context.AlunoGrupo.AnyAsync(ag => ag.GrupoId == grupoId && ag.AlunoId == alunoId))
            return await _context.Alunos.AsNoTracking().FirstOrDefaultAsync(a => a.Id == alunoId);
        else
            return null;
    }
}
