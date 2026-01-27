namespace Domain.Entities;

public class NotaFinal
{
    public int Id { get; set; }
    public int SessaoId { get; set; }
    public int AvaliadorId { get; set; }
    public int GrupoId { get; set; }
    public string DeviceHash { get; set; } = "";
    public DateTime DataEnvio { get; set; }
}