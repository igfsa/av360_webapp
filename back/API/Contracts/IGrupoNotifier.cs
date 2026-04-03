namespace Application.Contracts;

public interface IGrupoNotifier
{
    Task GrupoAtualizadoAsync(int GrupoId);
}
