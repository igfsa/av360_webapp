using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class TurmaAdmDTO
{
    public int Id { get; set; }
    [StringLength(100)]
    public required string Cod { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal NotaMax { get; set; } = 0;
    public int? ProfessorId { get; set; }
    public string? ProfessorNome { get; set; } = "";
}