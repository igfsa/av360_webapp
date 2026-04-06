namespace Application.DTOs;
public class DashboardSessaoDTO
{
    public int SessaoId { get; set; }

    public int TotalAlunos { get; set; }
    public int Avaliaram { get; set; }
    public int Pendentes { get; set; }

    public List<AlunoDashboardDTO> Alunos { get; set; } = [];
    public List<CriterioDashboardDTO> Criterios { get; set; } = [];
    public List<GrupoDashboardDTO> Grupos { get; set; } = [];
}
