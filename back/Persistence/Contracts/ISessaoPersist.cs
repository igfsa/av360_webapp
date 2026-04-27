using Domain.Entities;

namespace Persistence.Contracts;

public interface ISessaoPersist
{
    Task<Sessao?> GetSessaoIdAsync(int SessaoId);
    Task<Sessao?> GetSessaoAtivaTurmaIdAsync(int TurmaId);
    Task<Sessao?> GetValidaSessaoChavePubAsync(string token);
}