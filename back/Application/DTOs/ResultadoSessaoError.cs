namespace Application.DTOs;

public class ResultadoSessaoErrorDTO
{
    public string Tipo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Erro { get; set; } = null!;
}