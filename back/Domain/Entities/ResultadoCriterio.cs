using Domain.Exceptions;

namespace Domain.Entities;

public class ResultadoCriterio
{
    private ResultadoCriterio () {}
    public ResultadoCriterio (Criterio criterio, ResultadoSessao resultado)
    {
        CriterioId = criterio.Id;
        ResultadoSessaoId = resultado.Id;
        ResultadoSessao = resultado
                ?? throw new NotFoundException("Resultado Sessão não encontrada");
        Nome = criterio.Nome;
    }
    public int Id { get; private set; }
    public int CriterioId { get; private set; }
    public int ResultadoSessaoId { get; private set; }
    public ResultadoSessao ResultadoSessao { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
}
