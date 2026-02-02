using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class AvaliacaoEnvioDTO
{
    public int SessaoId { get; set; }
    public int GrupoId { get; set; }
    public int Avaliador { get; set; }
    public string DeviceHash { get; set; } = "";
    public List<AvaliacaoItemDTO> Itens { get; set; } = [];
}