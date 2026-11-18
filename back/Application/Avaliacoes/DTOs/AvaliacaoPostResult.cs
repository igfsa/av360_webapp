namespace Application.DTOs;

public class AvaliacaoPostResultDTO
{
    public int Total { get; set; }
    public int Sucesso { get; set; }
    public int Falhas { get; set; }
    public List<AvaliacaoPostErrorDTO> Erros { get; set; } = [];
}
