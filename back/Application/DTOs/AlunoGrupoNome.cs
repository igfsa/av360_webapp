namespace Application.DTOs;

public class AlunoGrupoNomeDTO
{
    public int AlunoId { get; set; }
    public string Nome { get; set; } = null!;
    public int? GrupoId { get; set; }
    public string? GrupoNome { get; set; } = null!;
    public int TurmaId { get; set; }
    public string TurmaCod { get; set; } = null!;
}