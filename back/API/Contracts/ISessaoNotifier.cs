using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts;
public interface ISessaoNotifier
{
    Task NovaSessao(int sessaoId);    
}
