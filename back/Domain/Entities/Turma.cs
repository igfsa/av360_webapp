namespace Domain.Entities;

public class Turma
{
    public int Id { get; set; }
    public string Cod { get; set; }
    public decimal? NotaMax { get; set; }
    public IEnumerable<Aluno>? Alunos { get; set; }
    public IEnumerable<Criterio>? Criterios { get; set; }
}
