namespace Application.Contracts;

public interface ITurmaNotifier
{
    Task TurmaAtualizadaAsync(int turmaId);
    Task AlunoTurmaAtualizada(int turmaId);
    Task CriterioTurmaAtualizada(int turmaId);
}
