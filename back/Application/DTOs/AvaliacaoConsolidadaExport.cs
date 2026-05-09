using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs;

public class AvaliacaoConsolidadaExportDTO
{
    public string Aluno { get; set; } = null!;
    [Column(TypeName = "decimal(5,2)")]
    public decimal Nota { get; set; }
}
