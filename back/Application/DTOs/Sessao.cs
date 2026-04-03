namespace Application.DTOs;

public class SessaoDTO
{
    public int Id { get; set; }
    public int TurmaId { get; set; }
    public TurmaDTO? Turma { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string TokenPublico { get; set; } = null!;
    public bool Ativo { get; set; } = false;
}
