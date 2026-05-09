namespace Application.Contracts;

public interface ISessaoNotifier
{
    Task NovaSessao(int sessaoId);
    Task SessaoAtualizada(int sessaoId);
    Task SessaoFinalizada(int sessaoId);
}
