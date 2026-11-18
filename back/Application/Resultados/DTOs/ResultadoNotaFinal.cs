namespace Application.DTOs;

public class ResultadoNotaFinalDTO
{
    public int Id { get;  set; }
    public int NotaFinalId { get;  set; }
    public int ResultadoSessaoId { get;  set; }
    public ResultadoSessaoDTO? ResultadoSessao { get;  set; }
    public int AvaliadorId { get;  set; }
    public string AvaliadorNome { get;  set; } = null!;
    public int GrupoResId { get;  set; }
    public string GrupoNome { get;  set; } = null!;
    public DateTime DataEnvio { get; set; }
    public string? ComentarioAluno { get; set; }
    private readonly List<ResultadoNotaParcialDTO> _notasParciais = [];
    public IReadOnlyCollection<ResultadoNotaParcialDTO> NotasParciais => _notasParciais;
}
