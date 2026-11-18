using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class AlunoGrupoPersist(APIContext context) : IAlunoGrupoPersist
{
    private readonly APIContext _context = context;

    public async Task<Aluno[]> GetAlunosGrupoId(int grupoId)
    {
        return await _context.Alunos
            .AsNoTracking()
            .Where(a => _context.AlunoGrupo
                .Any(ag => ag.GrupoId == grupoId && ag.AlunoId == a.Id))
            .OrderBy(a => a.Nome)
            .ToArrayAsync();
    }
    public async Task<AlunoGrupo[]> GetAlunosGrupoTurmaId(int turmaId)
    {
        return await _context.AlunoGrupo
            .AsNoTracking()
            .Where(ag => ag.TurmaId == turmaId)
            .ToArrayAsync();
    }
}
