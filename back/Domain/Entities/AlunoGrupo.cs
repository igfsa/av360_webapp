using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class AlunoGrupo
{
    public int AlunoId { get; set; }
    public int GrupoId { get; set; }
    public int TurmaId { get; set; }
}
