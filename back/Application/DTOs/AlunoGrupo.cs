namespace Application.DTOs;

public class AlunoGrupoDTO
{
    public int TurmaId { get; set; }
    public int GrupoId { get; set; }
    public List<int> AlunoIds { get; set; } = [];
}