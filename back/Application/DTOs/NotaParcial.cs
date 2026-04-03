using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs;

public class NotaParcialDTO
{
    public int Id { get; set; }
    public int NotaFinalId { get; set; }
    public NotaFinalDTO? NotaFinal { get; set; }
    public int AvaliadoId { get; set; }
    public AlunoDTO? Avaliado { get; set; }
    public int CriterioId { get; set; }
    public CriterioDTO? Criterio { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Nota { get; set; }
}
