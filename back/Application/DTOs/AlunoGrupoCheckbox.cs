using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class AlunoGrupoCheckboxDTO
{
    public int AlunoId { get; set; }
    public string Nome { get; set; }

    public bool Selecionado { get; set; }
    public bool Desabilitado { get; set; }

    public int? GrupoAtualId { get; set; }
}