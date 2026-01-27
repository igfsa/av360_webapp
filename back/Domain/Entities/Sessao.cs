using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Sessao
{
    public int Id { get; set; }
    public int TurmaId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public Guid TokenPublico { get; set; }
    public bool Ativo { get; set; }
}
