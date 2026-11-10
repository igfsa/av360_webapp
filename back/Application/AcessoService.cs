using Application.Contracts;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class AcessoService(
    ICurrentUser user, 
    ITurmaPersist turmaPersist,
    ICriterioPersist criterioPersist) : IAcessoService
{
    private readonly ICurrentUser _user = user;

    private readonly ITurmaPersist _turmaPersist = turmaPersist;

    private readonly ICriterioPersist _criterioPersist = criterioPersist;

    public async Task GarantirTurmaAsync(int turmaId)
    {
        if (_user.IsAdmin) return;
        var turma = await _turmaPersist.GetTurmaIdAsync(turmaId)
            ?? throw new NotFoundException("Turma inválida");
        if(turma.ProfessorId != _user.ProfessorId) 
            throw new ForbiddenException("Sem acesso a esta turma.");
    }
    public async Task GarantirCriterioAsync(int criterioId)
    {
        if (_user.IsAdmin) return;
        var criterio = await _criterioPersist.GetCriterioIdAsync(criterioId)
            ?? throw new NotFoundException("Criterio inválido");
        if(criterio.ProfessorId != _user.ProfessorId) 
            throw new ForbiddenException("Sem acesso a este criterio.");
    }

}