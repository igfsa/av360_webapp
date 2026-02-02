namespace Domain.Entities;

public class Turma
{
    public int Id { get; set; }
    public required string Cod { get; set; }
    public decimal? NotaMax { get; set; }
    public IEnumerable<AlunoTurma>? AlunoTurma { get; set; }
    public IEnumerable<CriterioTurma>? CriterioTurma { get; set; }
    public IEnumerable<Grupo>? Grupos { get; set; }
}
