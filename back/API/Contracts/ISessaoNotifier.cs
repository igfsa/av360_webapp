namespace Application.Contracts;

public interface ISessaoNotifier
{
    Task NovaSessao(int sessaoId);
}
