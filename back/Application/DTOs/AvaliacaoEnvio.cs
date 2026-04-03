namespace Application.DTOs;

public class AvaliacaoEnvioDTO
{
    public int SessaoId { get; set; }
    public int GrupoId { get; set; }
    public int AvaliadorId { get; set; }
    public AlunoDTO Avaliador { get; set; } = null!;
    public string DeviceHash { get; set; } = "";
    public IEnumerable<AvaliacaoItemDTO>? Itens { get; set; }
}