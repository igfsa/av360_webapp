using Domain.Exceptions;

namespace Domain.Entities;

public class Criterio
{
    private Criterio(){}
    public Criterio(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
    }
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    private readonly List<Turma> _turmas = new();
    public IReadOnlyCollection<Turma> Turmas => _turmas;

    public void AtualizarCriterio(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
    }
}