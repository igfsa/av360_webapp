using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Domain.Entities;

namespace Application.DTOs;

public class NotaParcialDTO
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public AlunoDTO Aluno { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Nota { get; set; } = 0;
    public int TurmaId { get; set; }
    public TurmaDTO Turma { get; set; }
    public int CriterioId { get; set; }
    public CriterioDTO Criterio { get; set; }
    public int AvaliadorId { get; set; }
    public AlunoDTO Avaliador { get; set; }
    public int NotaFinalId { get; set; }
    public NotaFinalDTO NotaFinal { get; set; }
}
