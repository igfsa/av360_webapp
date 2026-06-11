using Domain.Exceptions;

namespace Domain.Entities;

public class ResultadoAluno
{
    private ResultadoAluno () {}
    public ResultadoAluno (Aluno aluno, ResultadoSessao resSessao, ResultadoGrupo? resGrupo)
    {
    AlunoId = aluno.Id;
    ResultadoSessaoId = resSessao.Id;
    ResultadoSessao = resSessao
            ?? throw new NotFoundException("Resultado Sessão não encontrada");
    Nome = aluno.Nome;
    if (resGrupo != null)
        {
            ResultadoGrupoId = resGrupo.Id;
            ResultadoGrupo = resGrupo
                ?? throw new NotFoundException("Resultado Grupo não encontrada");
        }
    }
    public int Id { get; private set; }
    public int AlunoId { get; private set; } 
    public int ResultadoSessaoId { get; private set; } 
    public ResultadoSessao ResultadoSessao { get; private set; } = null!;
    public int ResultadoGrupoId { get; private set; } 
    public ResultadoGrupo? ResultadoGrupo { get; private set; }
    public string Nome { get; private set; } = null!;
}