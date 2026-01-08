using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

using Domain.Entities;

public class TurmaDTO
{
    public int Id { get; set; }
    [StringLength(30)]
    public string Cod { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal NotaMax { get; set; } = 0;
    [JsonIgnore]
    public IEnumerable<AlunoDTO>? Alunos { get; set; }
    [JsonIgnore]
    public IEnumerable<CriterioDTO>? Criterio { get; set; }
}