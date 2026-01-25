using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class TurmaCriterioDTO
{
    public int turmaId { get; set; }
    public List<int> criterioIds { get; set; } = new();
}