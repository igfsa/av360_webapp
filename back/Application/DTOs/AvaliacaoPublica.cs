using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class AvaliacaoPublicaDTO
{
    public int SessaoId { get; set; }
    public int TurmaId { get; set; }
    public IEnumerable<GrupoDTO> Grupos { get; set; } = [];
    public IEnumerable<CriterioDTO> Criterios { get; set; } = [];
}