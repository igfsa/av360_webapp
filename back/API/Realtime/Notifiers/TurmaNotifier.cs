using API.Hubs;
using Application.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifier;

public class TurmaNotifier(IHubContext<TurmaHub> hub) : ITurmaNotifier
{
    private readonly IHubContext<TurmaHub> _hub = hub;

    public async Task TurmaAtualizada(int turmaId)
    {
        await _hub.Clients.All
            .SendAsync("TurmaAtualizada", turmaId);
    }

    public async Task AlunoTurmaAtualizada(int turmaId)
    {
        await _hub.Clients
            .Group($"turma-{turmaId}")
            .SendAsync("AlunoTurmaAtualizada", turmaId);
    }

    public async Task CriterioTurmaAtualizada(int turmaId)
    {
        await _hub.Clients
            .Group($"turma-{turmaId}")
            .SendAsync("CriterioTurmaAtualizada", turmaId);
    }

    public async Task SessaoTurmaCriada(int turmaId)
    {
        await _hub.Clients.All
            .SendAsync("SessaoTurmaCriada", turmaId);
    }
}
