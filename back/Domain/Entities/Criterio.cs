using System.Runtime.CompilerServices;
using Domain.Exceptions;

[assembly: InternalsVisibleTo("Persistence")]

namespace Domain.Entities;

public class Criterio
{
    internal Criterio(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
    private Criterio() { }
    public Criterio(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
    }
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    private readonly List<Turma> _turmas = [];
    public IReadOnlyCollection<Turma> Turmas => _turmas;

    public void AtualizarCriterio(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
    }
}