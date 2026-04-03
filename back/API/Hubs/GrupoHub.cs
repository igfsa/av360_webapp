using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class GrupoHub : Hub
{
    public async Task GrupoAtualizado(int grupoId)
    {
        await Clients.All.SendAsync("GrupoAtualizado", grupoId);
    }
}
