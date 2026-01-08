namespace Domain.Entities;

public class NotaParcial
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; }
    public decimal Nota { get; set; }
    public int TurmaId { get; set; }
    public Turma Turma { get; set; }
    public int CriterioId { get; set; }
    public Criterio Criterio { get; set; }
    public int AvaliadorId { get; set; }
    public Aluno Avaliador { get; set; }
    public int NotaFinalId { get; set; }
    public NotaFinal NotaFinal { get; set; }
}
