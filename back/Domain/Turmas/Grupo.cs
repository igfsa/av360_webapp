using Domain.Exceptions;

namespace Domain.Entities;

public class Grupo
{
    private Grupo() { }
    public Grupo(string nome, Turma turma)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
        Turma = turma
            ?? throw new BusinessException("Turma é obrigatória");
        TurmaId = turma.Id;
    }
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public Turma Turma { get; private set; } = null!;
    public int TurmaId { get; private set; }
    public void AtualizarGrupo(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
    }
}
