using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CriterioDTO
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Nome { get; set; } = "";
    public bool Nativo { get; private set; } = false;
}