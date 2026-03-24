using Domain.Exceptions;

namespace Domain.Entities;

public class NotaParcial
{
    private NotaParcial(){}
    public NotaParcial(NotaFinal nFinal, Aluno avaliado, Criterio criterio, decimal nota)
    {
        NotaFinal = nFinal;
        NotaFinalId = nFinal.Id;
        Avaliado = avaliado;
        AvaliadoId = avaliado.Id;
        Criterio = criterio;
        CriterioId = criterio.Id;
        Nota = nota;
    }
    public int Id { get; set; }
    public int NotaFinalId { get; private set; }
    public readonly NotaFinal NotaFinal = null!;
    public int AvaliadoId { get; private set; }
    public readonly Aluno Avaliado = null!;
    public int CriterioId { get; private set; }
    public readonly Criterio Criterio = null!;
    public decimal Nota { get; set; }
}
