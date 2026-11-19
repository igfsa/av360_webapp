namespace Application.DTOs;

public class CriterioDashboardDTO
{
    public int CriterioId { get; set; }
    public string Nome { get; set; } = null!;
    public decimal MediaGlobal { get; set; }
    public int TotalNotas { get; set; }
}