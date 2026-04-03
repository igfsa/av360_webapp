using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class CriterioHub : Hub
{
    public async Task CriterioAtualizado(int criterioId)
    {
        await Clients.All.SendAsync("CriterioAtualizado", criterioId);
    }
}
