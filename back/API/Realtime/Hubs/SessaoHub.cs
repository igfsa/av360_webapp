using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class SessaoHub : Hub
{
    public async Task NovaSessao(int sessaoId)
    {
        await Clients.All.SendAsync("SessoesAtualizadas", sessaoId);
    }
    public async Task SessaoAtualizada(int sessaoId)
    {
        await Clients.All.SendAsync("SessaoAtualizada", sessaoId);
    }

    [HubMethodName("AcessarSessao")]
    public async Task AcessarTurmaSessao(int sessaoId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Sessao-{sessaoId}");
    }
}
