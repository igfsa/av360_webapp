namespace Application.DTOs;

public class GrupoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public int TurmaId { get; set; }
    public TurmaDTO? Turma { get; set; }
}
