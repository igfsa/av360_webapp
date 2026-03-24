using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class AlunoGrupoDTO
{
    public int turmaId { get; set; }
    public int grupoId { get; set; }
    public List<int> alunoIds { get; set; } = new();
}