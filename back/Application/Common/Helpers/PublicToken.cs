using System.Security.Cryptography;

namespace Application.Helpers;

public class PublicToken
{
    public static string GeneratePublicToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(16))
            .ToLower();
    }
}
