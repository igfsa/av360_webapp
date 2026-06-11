using Domain.Exceptions;

namespace Domain.Entities;

public class ResultadoNotaParcial
{
    private ResultadoNotaParcial () {}
    public ResultadoNotaParcial (
                                    ResultadoNotaFinal resultadoNFinal, 
                                    NotaParcial nParcial, 
                                    ResultadoAluno avaliadoRes, 
                                    ResultadoCriterio criterioRes)
    {
        NotaParcialId = nParcial.Id;
        ResultadoNotaFinalId = resultadoNFinal.Id;
        ResultadoNotaFinal = resultadoNFinal
                ?? throw new NotFoundException("Resultado Nota final não encontrada");
        AvaliadoResId = avaliadoRes.Id;
        AvaliadoResNome = avaliadoRes.Nome;
        CriterioResId = criterioRes.Id;
        CriterioResNome = criterioRes.Nome;
        Nota = nParcial.Nota;

    }
    public int Id { get;  private set; }
    public int NotaParcialId { get; private set; }
    public int ResultadoNotaFinalId { get; private set; }
    public ResultadoNotaFinal ResultadoNotaFinal { get; private set; } = null!;
    public int AvaliadoResId { get; private set; }
    public string AvaliadoResNome { get; private set; } = null!;
    public int CriterioResId { get; private set; }
    public string CriterioResNome { get; private set; } = null!;
    public decimal Nota { get; private set; }
}
