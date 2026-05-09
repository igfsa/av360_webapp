using API.Hubs;
using Application.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifier;

public class CriterioNotifier(IHubContext<CriterioHub> hub) : ICriterioNotifier
{
    private readonly IHubContext<CriterioHub> _hub = hub;

    public async Task CriterioAtualizado(int criterioId)
    {
        await _hub.Clients.All
            .SendAsync("CriterioAtualizado", criterioId);
    }
}
