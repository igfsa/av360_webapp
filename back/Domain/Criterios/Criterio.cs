using Domain.Exceptions;

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
    public Criterio(string nome, Professor? professor, bool nativo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
        ProfessorId = professor?.Id;
        Professor = professor;
        Nativo = nativo;
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
    public void AtualizarCriterioAdm(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
    }
}