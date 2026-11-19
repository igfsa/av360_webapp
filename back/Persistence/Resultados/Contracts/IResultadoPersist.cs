using Domain.Entities;

namespace Persistence.Contracts;

public interface IResultadoPersist
{
    Task<ResultadoSessao?> GetResultadoSessaoIdAsync(int sessaoId);
    Task<ResultadoAluno[]> GetAlunosResultadoSessaoIdAsync(int resultadoSessaoId);
    Task<ResultadoCriterio[]> GetCriteriosResultadoSessaoIdAsync(int resultadoSessaoId);
    Task<ResultadoGrupo[]> GetGruposResultadoSessaoIdAsync(int resultadoSessaoId);
    Task<ResultadoNotaFinal[]> GetNotasFinalResultadoSessaoIdAsync(int resultadoSessaoId);
    Task<ResultadoNotaParcial[]> GetNotaParcialResultadoSessaoIdAsync(int resultadoSessaoId);
}