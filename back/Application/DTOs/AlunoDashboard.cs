namespace Application.DTOs;

public class AlunoDashboardDTO
{
    public int AlunoId { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Media { get; set; }
    public int GrupoId { get; set; }
    public List<CriterioAlunoDashboardDTO> CriterioAluno { get; set; } = [];
    public bool Avaliou { get; set; }
    public int TotalNotas { get; set; }
}