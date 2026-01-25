using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class CsvImportRequestDTO
{
    public IFormFile Arquivo { get; set; } = default!;
    public string ColunaNome { get; set; }
}
