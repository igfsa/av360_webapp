using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class AlunoDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = null!;
    public IEnumerable<TurmaDTO>? Turmas { get; set; }
}