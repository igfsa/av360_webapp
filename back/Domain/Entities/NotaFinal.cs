namespace Domain.Entities;

public class NotaFinal
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public required Aluno Aluno { get; set; }
    public decimal Nota { get; set; }
    public int TurmaId { get; set; }
    public required Turma Turma { get; set; }
    public int CriterioId { get; set; }
    public required Criterio Criterio { get; set; }
    public IEnumerable<NotaParcial>? NotasParciais { get; set; }
}