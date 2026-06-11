using Domain.Exceptions;

namespace Domain.Entities;

public class ResultadoCriterioDTO
{
    public int Id { get; private set; }
    public int CriterioId { get; private set; }
    public int ResultadoSessaoId { get; private set; }
    public ResultadoSessao ResultadoSessao { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
}
