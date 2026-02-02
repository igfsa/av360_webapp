using API.Hubs;
using Application.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifier;

public class AvaliacaoNotifier(IHubContext<AvaliacaoHub> hub) : IAvaliacaoNotifier
{
    private readonly IHubContext<AvaliacaoHub> _hub = hub;

    public async Task NovaAvaliacao(int sessaoId)
    {
        await _hub.Clients.All
            .SendAsync("AvaliacoesAtualizadas", sessaoId);
    }

}
