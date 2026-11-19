namespace Application.Contracts;

public interface IGrupoNotifier
{
    Task GrupoAtualizado(int GrupoId);
}
