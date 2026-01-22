using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class CriterioHub : Hub
{
    public async Task CriterioAtualizado(int criterioId)
    {
        await Clients.All.SendAsync("CriterioAtualizado", criterioId);
    }
}
