using API.Hubs;
using Application.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifier;

public class GrupoNotifier(IHubContext<GrupoHub> hub) : IGrupoNotifier
{
    private readonly IHubContext<GrupoHub> _hub = hub;

    public async Task GrupoAtualizadoAsync(int grupoId)
    {
        await _hub.Clients.All
            .SendAsync("GrupoAtualizado", grupoId);
    }
}
