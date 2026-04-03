namespace Application.DTOs;

public class NotaFinalDTO
{
    public int Id { get; set; }
    public int SessaoId { get; set; }
    public SessaoDTO? Sessao { get; set; }
    public int AvaliadorId { get; set; }
    public AlunoDTO? Avaliador { get; set; }
    public int GrupoId { get; set; }
    public string DeviceHash { get; set; } = "";
    public DateTime DataEnvio { get; set; }
    public IEnumerable<NotaParcialDTO>? NotasParciais { get; set; }
}