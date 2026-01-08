using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class AlunoTurma
{
    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; }
    public int TurmaId { get; set; }
    public Turma Turma { get; set; }
    
}
