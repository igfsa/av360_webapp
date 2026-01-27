using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Grupo
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public int TurmaId { get; set; }
}
