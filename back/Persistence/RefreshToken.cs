using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Contracts;
using Persistence.Context;

namespace Persistence;

public class RefreshTokenPersist(APIContext context) : IRefreshTokenPersist
{
    private readonly APIContext _context = context;

    public async Task<RefreshToken?> GetRefreshToken(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);
    }
}
