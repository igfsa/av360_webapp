using API.Hubs;
using Application.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifier;

public class ProfessorNotifier(IHubContext<ProfessorHub> hub) : IProfessorNotifier
{
    private readonly IHubContext<ProfessorHub> _hub = hub;

    public async Task ProfessorAtualizado(int professorId)
    {
        await _hub.Clients.All
            .SendAsync("ProfessorAtualizado", professorId);
    }
}
