namespace Application.Contracts;

public interface IAcessoService
{
    Task GarantirTurmaAsync(int turmaId);
    Task GarantirCriterioAsync(int criterioId);
}