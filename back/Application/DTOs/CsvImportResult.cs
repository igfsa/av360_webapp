using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs;

public class CsvImportResultDTO
{
    public int Total { get; set; }
    public int Sucesso { get; set; }
    public int Falhas { get; set; }
    public List<CsvImportErrorDTO> Erros { get; set; } = [];
}

