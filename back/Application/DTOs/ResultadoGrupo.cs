namespace Application.DTOs;

public class ResultadoGrupoDTO
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public int ResultadoSessaoId { get; private set; }
    public ResultadoSessaoDTO ResultadoSessao { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
    private readonly List<ResultadoAlunoDTO> _alunos = [];
    public IReadOnlyCollection<ResultadoAlunoDTO> Alunos => _alunos;
}