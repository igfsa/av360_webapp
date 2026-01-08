namespace Domain.Entities;

public class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public IEnumerable<AlunoTurma>? AlunoTurma { get; set; }
    public IEnumerable<NotaFinal>? NotasFinais { get; set; }
    public IEnumerable<NotaParcial>? NotasParciais { get; set; }
}