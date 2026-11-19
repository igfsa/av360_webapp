using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class CsvImportRequestDTO
{
    public int TurmaId { get; set; }
    public IFormFile Arquivo { get; set; } = default!;
    public string ColunaNome { get; set; } = null!;
}
