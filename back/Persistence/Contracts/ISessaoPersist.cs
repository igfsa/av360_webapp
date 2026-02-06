using Domain.Entities;

namespace Persistence.Contracts;

public interface ISessaoPersist
{
    Task<Sessao[]> GetAllSessoesAsync();
    Task<Sessao> GetSessaoIdAsync(int SessaoId);
    Task<Sessao[]> GetSessoesTurmaIdAsync(int turmaId);
    Task<Sessao?> GetSessaoAtivaTurmaIdAsync(int TurmaId);
}