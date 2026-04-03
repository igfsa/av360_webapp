namespace Application.Contracts;

public interface ICriterioNotifier
{
    Task CriterioAtualizadoAsync(int CriterioId);
}
