using Domain.Exceptions;

namespace Domain.Entities;

public class ResultadoNotaFinal
{
    private ResultadoNotaFinal () {}
    public ResultadoNotaFinal (ResultadoSessao resultado, NotaFinal nFinal, ResultadoAluno avaliador, ResultadoGrupo grupo)
    {
        NotaFinalId = nFinal.Id;
        ResultadoSessaoId = resultado.Id;
        ResultadoSessao = resultado
                ?? throw new NotFoundException("Resultado Sessão não encontrada");
        AvaliadorResId = avaliador.Id;
        AvaliadorNome = avaliador.Nome;
        GrupoResId = grupo.Id;
        GrupoNome = grupo.Nome;
        DataEnvio = nFinal.DataEnvio;
        ComentarioAluno = nFinal.ComentarioAluno;
    }
    public int Id { get;  private set; }
    public int NotaFinalId { get; private set; }
    public int ResultadoSessaoId { get; private set; }
    public ResultadoSessao ResultadoSessao { get; private set; } = null!;
    public int AvaliadorResId { get; private set; }
    public string AvaliadorNome { get; private set; } = null!;
    public int GrupoResId { get; private set; }
    public string GrupoNome { get; private set; } = null!;
    public DateTime DataEnvio { get; private set; }
    public string? ComentarioAluno { get; set; }
    private readonly List<ResultadoNotaParcial> _notasParciais = [];
    public IReadOnlyCollection<ResultadoNotaParcial> NotasParciais => _notasParciais;

    public ResultadoNotaParcial AdicionarNotaParcial(NotaParcial nParcial, ResultadoAluno avaliadorRes, ResultadoCriterio criterioRes)
    {
        var np = new ResultadoNotaParcial(this, nParcial, avaliadorRes, criterioRes);
        _notasParciais.Add(np);
        
        return np;
    }
}
