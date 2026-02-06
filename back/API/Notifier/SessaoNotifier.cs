using API.Hubs;
using Application.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifier;

public class SessaoNotifier(IHubContext<SessaoHub> hub) : ISessaoNotifier
{
    private readonly IHubContext<SessaoHub> _hub = hub;

    public async Task NovaSessao(int sessaoId)
    {
        await _hub.Clients.All
            .SendAsync("SessoesAtualizadas", sessaoId);
    }

}
