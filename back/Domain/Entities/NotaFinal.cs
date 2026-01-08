namespace Domain.Entities;

public class NotaFinal
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; }
    public decimal Nota { get; set; }
    public int TurmaId { get; set; }
    public Turma Turma { get; set; }
    public int CriterioId { get; set; }
    public Criterio Criterio { get; set; }
    public IEnumerable<NotaParcial>? NotasParciais { get; set; }
}