using Domain.Exceptions;

namespace Domain.Entities;

public class Turma
{
    private Turma (){}
    public Turma(string cod, decimal notaMax)
    {
        if (string.IsNullOrWhiteSpace(cod))
            throw new BusinessException("Cod é obrigatório");

        Cod = cod;
        NotaMax = notaMax;
    }
    public int Id { get; set; }
    public string Cod { get; private set; } = null!;
    public decimal NotaMax { get; private set; }
    private readonly List<Aluno> _alunos = new();
    public IReadOnlyCollection<Aluno> Alunos => _alunos;
    private readonly List<Criterio> _criterios = new();
    public IReadOnlyCollection<Criterio> Criterios => _criterios;
    private readonly List<Grupo> _grupos = new();
    public IReadOnlyCollection<Grupo> Grupos => _grupos;
    
    public void AtualizarTurma(string cod, decimal notaMax)
    {
        if (string.IsNullOrWhiteSpace(cod))
            throw new BusinessException("Cod é obrigatório");

        Cod = cod;
        NotaMax = notaMax;
    }

    public void AdicionarAluno(Aluno aluno)
    {
        if (_alunos.Any(a => a.Id == aluno.Id))
            throw new BusinessException("Aluno já matriculado.");
        Console.WriteLine(aluno.Nome);
        _alunos.Add(aluno);
    }

    public void RemoverAluno(int alunoId)
    {
        var aluno = _alunos.FirstOrDefault(a => a.Id == alunoId);
        if (aluno == null)
            throw new BusinessException("Aluno não encontrado.");

        _alunos.Remove(aluno);
    }

    public void AtualizarCriterios(IEnumerable<Criterio> novosCriterios)
    {
        var novosIds = novosCriterios.Select(c => c.Id).ToHashSet();

        // remover
        _criterios.RemoveAll(c => !novosIds.Contains(c.Id));

        // adicionar
        foreach (var criterio in novosCriterios)
        {
            if (!_criterios.Any(c => c.Id == criterio.Id))
                _criterios.Add(criterio);
        }
    }

    public void AdicionarGrupo(Grupo grupo)
    {
        if (_grupos.Any(a => a.Id == grupo.Id))
            throw new BusinessException("Equipe já vinculada.");

        _grupos.Add(grupo);
    }

    public void RemoverGrupo(int grupoId)
    {
        var grupo = _grupos.FirstOrDefault(a => a.Id == grupoId);
        if (grupo == null)
            throw new BusinessException("Equipe não vinculada.");

        _grupos.Remove(grupo);
    }
}
