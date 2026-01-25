using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs;

public class CsvImportErrorDTO
{
    public int Linha { get; set; }
    public string? Nome { get; set; }
    public string Erro { get; set; } = "";
}

