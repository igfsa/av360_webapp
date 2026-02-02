using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class AvaliacaoItemDTO
{
    public int AvaliadoId { get; set; }
    public int CriterioId { get; set; }
    public decimal Nota { get; set; }
}