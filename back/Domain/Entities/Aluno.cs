using System.Text.Json.Serialization;
using Domain.Exceptions;

namespace Domain.Entities;

public class Aluno
{
    private Aluno(){}
    public Aluno(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");

        Nome = nome;
    }

    public int Id { get; set; }
    public string Nome { get; private set; } = null!;
    private readonly List<Turma> _turmas = new();
    public IReadOnlyCollection<Turma> Turmas => _turmas;
    public void AtualizarAluno(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");

        Nome = nome;
    }
}