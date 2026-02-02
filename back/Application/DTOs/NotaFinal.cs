using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Domain.Entities;

namespace Application.DTOs;

public class NotaFinalDTO
{
    public int Id { get; set; }
    public int SessaoId { get; set; }
    public int AvaliadorId { get; set; }
    public int GrupoId { get; set; }
    public string DeviceHash { get; set; } = "";
    public DateTime DataEnvio { get; set; }
    [JsonIgnore]
    public IEnumerable<NotaParcial>? NotasParciais { get; set; }
}