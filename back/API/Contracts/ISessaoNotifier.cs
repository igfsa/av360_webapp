namespace Application.Contracts;

public interface ISessaoNotifier
{
    Task NovaSessao(int sessaoId);
    Task SessaoAtualizada(int sessaoId);
}
