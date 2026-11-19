using Domain.Entities;

namespace Persistence.Contracts;

public interface IRefreshTokenPersist
{
    Task<RefreshToken?> GetRefreshToken(string token);
}
