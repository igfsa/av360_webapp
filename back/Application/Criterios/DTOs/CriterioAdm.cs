using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CriterioAdmDTO
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Nome { get; set; } = "";
    public bool Nativo { get; set; } = false;
    public int? ProfessorId { get; set; }
    public string? ProfessorNome { get; set; } = "";
}