using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class SessaoHub : Hub
{
    public async Task NovaSessao (int sessaoId)
    {
        await Clients.All.SendAsync("SessoesAtualizadas", sessaoId);
    }

    [HubMethodName("AcessarSessao")]
    public async Task AcessarTurmaSessao(int sessaoId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Sessao-{sessaoId}");
    }
}
