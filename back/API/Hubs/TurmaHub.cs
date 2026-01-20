using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class TurmaHub : Hub
{
    public async Task TurmaAtualizada(int turmaId)
    {
        await Clients.All.SendAsync("TurmaAtualizada", turmaId);
    }
}
