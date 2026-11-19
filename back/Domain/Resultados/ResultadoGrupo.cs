using Domain.Exceptions;

namespace Domain.Entities;

public class ResultadoGrupo
{
    private ResultadoGrupo () {}
    public ResultadoGrupo (Grupo grupo, ResultadoSessao resultado)
    {
        GrupoId = grupo.Id;
        ResultadoSessaoId = resultado.Id;
        ResultadoSessao = resultado
                ?? throw new NotFoundException("Resultado Sessão não encontrada");
        Nome = grupo.Nome;
    }
    public int Id { get; private set; }
    public int GrupoId { get; private set; }
    public int ResultadoSessaoId { get; private set; }
    public ResultadoSessao ResultadoSessao { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
    private readonly List<ResultadoAluno> _alunos = [];
    public IReadOnlyCollection<ResultadoAluno> Alunos => _alunos;

    public ResultadoAluno AdicionarResAluno(ResultadoAluno aluno)
    {
        _alunos.Add(aluno);
        
        return aluno;
    }
}
