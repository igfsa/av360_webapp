using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class ProfessorDTO
{
    public int Id { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9]+$")]
    [MaxLength(100)]
    public string UserName { get; set; } = null!;
    
    [MaxLength(100)]
    public string Nome { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Perfil Perfil { get; set; }
}
