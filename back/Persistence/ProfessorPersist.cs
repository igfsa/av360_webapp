using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class ProfessorPersist(APIContext context) : IProfessorPersist
{
    private readonly APIContext _context = context;

    public async Task<Professor?> GetProfessorUser(string userName)
    {
        return await _context.Professores
            .FirstOrDefaultAsync(p => p.UserName == userName);
    }
}
