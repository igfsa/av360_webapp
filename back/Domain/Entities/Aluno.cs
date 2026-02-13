using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Aluno
{
    public int Id { get; set; }
    public required string Nome { get; set; }
}