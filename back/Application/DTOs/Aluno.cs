using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class AlunoDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength (100)]
    public string Nome { get; set; } = "";
    public IEnumerable<TurmaDTO>? Turmas { get; set; }
}