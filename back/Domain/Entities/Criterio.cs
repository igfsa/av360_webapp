namespace Domain.Entities;

public class Criterio
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int TurmaId { get; set; }
    public Turma? Turma { get; set; }
}