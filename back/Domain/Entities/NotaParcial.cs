namespace Domain.Entities;

public class NotaParcial
{
    public int Id { get; set; }
    public int NotaFinalId { get; set; }
    public int AvaliadoId { get; set; }
    public int CriterioId { get; set; }
    public decimal Nota { get; set; }
}
