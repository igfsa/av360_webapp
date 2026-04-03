namespace Domain.Entities;

public class NotaParcial
{
    private NotaParcial() { }
    public NotaParcial(NotaFinal nFinal, Aluno avaliado, Criterio criterio, decimal nota)
    {
        if (nFinal == null || avaliado == null || criterio == null)
            throw new ArgumentNullException();
        NotaFinal = nFinal;
        NotaFinalId = nFinal.Id;
        Avaliado = avaliado;
        AvaliadoId = avaliado.Id;
        Criterio = criterio;
        CriterioId = criterio.Id;
        Nota = nota;
    }
    public int Id { get; private set; }
    public int NotaFinalId { get; private set; }
    public NotaFinal NotaFinal { get;  private set; } = null!;
    public int AvaliadoId { get; private set; }
    public Aluno Avaliado { get;  private set; } = null!;
    public int CriterioId { get; private set; }
    public Criterio Criterio { get;  private set; } = null!;
    public decimal Nota { get;  private set; }
}
