namespace Application.DTOs;

public class CsvImportResultDTO
{
    public int Total { get; set; }
    public int Sucesso { get; set; }
    public int Falhas { get; set; }
    public List<CsvImportErrorDTO> Erros { get; set; } = [];
}
