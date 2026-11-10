using System.Runtime.CompilerServices;
using Domain.Exceptions;

[assembly: InternalsVisibleTo("Persistence")]

namespace Domain.Entities;

public class Criterio
{
    internal Criterio(int id, string nome, bool nativo)
    {
        Id = id;
        Nome = nome;
        Nativo = nativo;
    }
    private Criterio() { }
    public Criterio(string nome, Professor professor)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
        ProfessorId = professor.Id;
        Professor = professor;
    }
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public bool Nativo { get; private set; } = false;
    public int? ProfessorId { get; private set; }
    public Professor? Professor { get; private set; } = null;
    private readonly List<Turma> _turmas = [];
    public IReadOnlyCollection<Turma> Turmas => _turmas;

    public void AtualizarCriterio(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        if (Nativo)
            throw new ValidationException("Critério padrão não pode ser editado");
        Nome = nome;
    }
}