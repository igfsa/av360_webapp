namespace Application.Contracts;

public interface IAvaliacaoNotifier
{
    Task NovaAvaliacao(int sessaoId);
}
