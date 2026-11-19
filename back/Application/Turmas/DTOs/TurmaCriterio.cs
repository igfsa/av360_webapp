namespace Application.DTOs;

public class TurmaCriterioDTO
{
    public int TurmaId { get; set; }
    public List<int> CriterioIds { get; set; } = [];
}