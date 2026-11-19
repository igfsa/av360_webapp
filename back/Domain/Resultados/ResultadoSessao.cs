using Domain.Exceptions;

namespace Domain.Entities;

public class ResultadoSessao
{
    private ResultadoSessao() { }
    public ResultadoSessao(Sessao sessao, Turma turma)
    {
        SessaoId = sessao.Id;
        Sessao = sessao;
        TurmaId = turma.Id;
        TurmaCod = turma.Cod;
        NotaMaxima = turma.NotaMax;
        DataInicio = sessao.DataInicio;
        DataFim = sessao.DataFim ?? DateTime.UtcNow;
    }
    public int Id { get; private set; }
    public int SessaoId { get; private set; }
    public Sessao Sessao { get; private set; } = null!;
    public int TurmaId { get; private set; }
    public string TurmaCod { get; private set; } = null!;
    public decimal NotaMaxima { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public bool Inconsistencia { get; private set; } = true;
    private readonly List<ResultadoNotaFinal> _notasFinais = [];
    public IReadOnlyCollection<ResultadoNotaFinal> NotasFinais => _notasFinais;
    private readonly List<ResultadoGrupo> _grupos = [];
    public IReadOnlyCollection<ResultadoGrupo> Grupos => _grupos;
    private readonly List<ResultadoAluno> _alunos = [];
    public IReadOnlyCollection<ResultadoAluno> Alunos => _alunos;
    private readonly List<ResultadoCriterio> _criterios = [];
    public IReadOnlyCollection<ResultadoCriterio> Criterios => _criterios;

    public ResultadoNotaFinal AdicionarNotaFinal(NotaFinal nFinal, ResultadoAluno avaliadorRes, ResultadoGrupo grupo)
    {
        var nf = new ResultadoNotaFinal(this, nFinal, avaliadorRes, grupo);
        _notasFinais.Add(nf);
        
        return nf;
    }

    public ResultadoGrupo AdicionarGrupo(Grupo grupo)
    {
        var g = new ResultadoGrupo(grupo, this);
        _grupos.Add(g);
        
        return g;
    }

    public ResultadoAluno AdicionarAluno(Aluno aluno, ResultadoGrupo? grupo)
    {
        if (!_grupos.Any(g => g == grupo) && grupo != null)
            throw new NotFoundException("Grupo não encontrado na turma");

        var a = new ResultadoAluno(aluno, this, grupo);
        _alunos.Add(a);

        grupo?.AdicionarResAluno(a);

        return a;
    }

    public ResultadoCriterio AdicionarCriterio(Criterio criterio)
    {
        var c = new ResultadoCriterio(criterio, this);
        _criterios.Add(c);
        
        return c;
    }

    public void EditInconsistencia (bool inc)
    {
        Inconsistencia = inc;
    }
}
