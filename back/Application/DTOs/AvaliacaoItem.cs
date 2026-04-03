namespace Application.DTOs;

public class AvaliacaoItemDTO
{
    public int AvaliadoId { get; set; }
    public AlunoDTO Avaliado { get; set; } = null!;
    public int CriterioId { get; set; }
    public decimal Nota { get; set; }
}