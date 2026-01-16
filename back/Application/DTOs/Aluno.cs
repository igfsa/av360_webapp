using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class AlunoDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength (100)]
    public string Nome { get; set; }
    [JsonIgnore]
    public IEnumerable<TurmaDTO>? Turmas { get; set; }
    [JsonIgnore]
    public IEnumerable<NotaFinalDTO>? NotasFinais { get; set; }
    [JsonIgnore]
    public IEnumerable<NotaParcialDTO>? NotasParciais { get; set; }
}