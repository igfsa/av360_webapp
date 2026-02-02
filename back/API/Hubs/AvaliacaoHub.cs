using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class AvaliacaoHub : Hub
{
    public async Task NovaAvaliacao (int sessaoId)
    {
        await Clients.All.SendAsync("AvaliacoesAtualizadas", sessaoId);
    }

    [HubMethodName("AcessarAvaliacao")]
    public async Task AcessarTurmaAvaliacao(int sessaoId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Sessao-{sessaoId}");
    }
}
