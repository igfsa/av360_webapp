namespace Application.DTOs;

public class GrupoDashboardDTO
{
    public int GrupoId { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Media { get; set; }
    public int TotalNotas { get; set; }
}