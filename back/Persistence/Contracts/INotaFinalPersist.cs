using Domain.Entities;

namespace Persistence.Contracts;

public interface INotaFinalPersist
{
    Task<NotaFinal> GetNotaFinalIdAsync(int notaFinalId);
    Task<NotaFinal?> GetNotaFinalAlunoSessaoIdAsync(int alunoId, int sessaoId);
    Task<NotaFinal[]> GetNotaFinalGrupoSessaoIdAsync(int grupoId, int sessaoId);
    Task<NotaFinal?> GetNotaFinalHashAsync(string deviceHash, int sessaoId);
}