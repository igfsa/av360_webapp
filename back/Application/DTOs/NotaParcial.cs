using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Domain.Entities;

namespace Application.DTOs;

public class NotaParcialDTO
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    [JsonIgnore]
    public AlunoDTO Aluno { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Nota { get; set; } = 0;
    public int TurmaId { get; set; }
    [JsonIgnore]
    public TurmaDTO Turma { get; set; }
    public int CriterioId { get; set; }
    [JsonIgnore]
    public CriterioDTO Criterio { get; set; }
    public int AvaliadorId { get; set; }
    [JsonIgnore]
    public AlunoDTO Avaliador { get; set; }
    public int NotaFinalId { get; set; }
    [JsonIgnore]
    public NotaFinalDTO NotaFinal { get; set; }
}
