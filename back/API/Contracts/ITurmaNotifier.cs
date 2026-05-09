namespace Application.Contracts;

public interface ITurmaNotifier
{
    Task TurmaAtualizada(int turmaId);
    Task AlunoTurmaAtualizada(int turmaId);
    Task CriterioTurmaAtualizada(int turmaId);
}
