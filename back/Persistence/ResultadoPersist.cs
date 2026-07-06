using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class ResultadoPersist(APIContext context) : IResultadoPersist
{
    private readonly APIContext _context = context;
    public async Task<ResultadoSessao?> GetResultadoSessaoIdAsync(int sessaoId)
    {
        return await _context.SessoesResultados
            .Include(s => s.NotasFinais)
            .Include(s => s.Alunos)
            .Include(s => s.Grupos)
            .Include(s => s.Criterios)
            .FirstOrDefaultAsync(s => s.SessaoId == sessaoId);
    }

    public async Task<ResultadoAluno[]> GetAlunosResultadoSessaoIdAsync(int resultadoSessaoId)
    {
        return await _context.AlunosResultados
            .AsNoTracking()
            .Where(g => g.ResultadoSessao.Id == resultadoSessaoId)
            .ToArrayAsync();
    }

    public async Task<ResultadoCriterio[]> GetCriteriosResultadoSessaoIdAsync(int resultadoSessaoId)
    {
        return await _context.CriteriosResultados
            .AsNoTracking()
            .Where(g => g.ResultadoSessaoId == resultadoSessaoId)
            .ToArrayAsync();
    }

    public async Task<ResultadoGrupo[]> GetGruposResultadoSessaoIdAsync(int resultadoSessaoId)
    {
        return await _context.GruposResultados
            .AsNoTracking()
            .Where(g => g.ResultadoSessaoId == resultadoSessaoId)
            .Include(g => g.Alunos)
            .ToArrayAsync();
    }

    public async Task<ResultadoNotaFinal[]> GetNotasFinalResultadoSessaoIdAsync(int resultadoSessaoId)
    {
        return await _context.NotaFinaisResultados
            .AsNoTracking()
            .Where(nf => nf.ResultadoSessaoId == resultadoSessaoId)
            .Include(nf => nf.NotasParciais)
            .ToArrayAsync();
    }
    public async Task<ResultadoNotaParcial[]> GetNotaParcialResultadoSessaoIdAsync(int resultadoSessaoId)
    {
        return await _context.NotasParciaisResultados
            .Where(np => np.ResultadoNotaFinal.ResultadoSessaoId == resultadoSessaoId)
            .OrderByDescending(np => np.Id)
            .ToArrayAsync();
    }
}
