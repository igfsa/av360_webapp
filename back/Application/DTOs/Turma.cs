using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class TurmaDTO
{
    public int Id { get; set; }
    [StringLength(30)]
    public required string Cod { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal NotaMax { get; set; } = 0;
    public IEnumerable<AlunoDTO> Alunos { get; set; } = [];
    public IEnumerable<CriterioDTO> Criterios { get; set; } = [];
    public IReadOnlyCollection<GrupoDTO> Grupos { get; set; } = [];
}