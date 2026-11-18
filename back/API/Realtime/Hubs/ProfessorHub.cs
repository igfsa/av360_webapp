using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class ProfessorHub : Hub
{
    public async Task ProfessorAtualizado(int professorId)
    {
        await Clients.All.SendAsync("ProfessorAtualizado", professorId);
    }
}
