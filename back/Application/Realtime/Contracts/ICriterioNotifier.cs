namespace Application.Contracts;

public interface ICriterioNotifier
{
    Task CriterioAtualizado(int CriterioId);
}
