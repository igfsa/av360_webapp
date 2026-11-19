namespace Application.DTOs;

public class ResultadoAlunoDTO
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public int ResultadoSessaoId { get; set; } 
    public ResultadoSessaoDTO ResultadoSessao { get; set; } = null!;
    public int ResultadoGrupoId { get; set; } 
    public ResultadoGrupoDTO? ResultadoGrupo { get; set; }
    public string Nome { get; set; } = "";
}
