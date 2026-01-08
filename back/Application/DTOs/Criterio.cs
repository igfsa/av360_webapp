using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class CriterioDTO
{
    public int Id { get; set; }
   [StringLength (100)]
    public string Nome { get; set; }
    public int TurmaId { get; set; }
    [JsonIgnore]
    public IEnumerable<TurmaDTO>? Turma { get; set; }
}