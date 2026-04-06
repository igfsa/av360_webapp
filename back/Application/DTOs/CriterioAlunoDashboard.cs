namespace Application.DTOs;

public class CriterioAlunoDashboardDTO
{
    public int CriterioId { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Media { get; set; }
    public int TotalNotas { get; set; }
}