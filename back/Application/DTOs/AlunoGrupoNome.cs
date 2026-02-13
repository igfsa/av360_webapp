using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class AlunoGrupoNomeDTO
{
    public int AlunoId { get; set; }
    public string Nome { get; set; } = "";
    public int? GrupoId { get; set; }
    public string? GrupoNome { get; set; } = "" ;
    public int TurmaId { get; set; }
    public string TurmaCod { get; set; } = "" ;
}