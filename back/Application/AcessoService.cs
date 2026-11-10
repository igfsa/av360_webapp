using Application.Contracts;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class AcessoService(ICurrentUser user, ITurmaPersist turmaPersist) : IAcessoService
{
    public async Task GarantirTurmaAsync(int turmaId)
    {
        if (user.IsAdmin) return;
        var turma = await turmaPersist.GetTurmaIdAsync(turmaId)
            ?? throw new NotFoundException("Turma inválida");
        if(turma.ProfessorId != user.ProfessorId) 
            throw new ForbiddenException("Sem acesso a esta turma.");
    }

}