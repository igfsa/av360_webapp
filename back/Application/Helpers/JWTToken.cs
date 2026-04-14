using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.Helpers;

public class JWTToken()
{

    public static string GenerateJWTToken()
    {
        var issuer = Environment.GetEnvironmentVariable("AV360_ISSUER");
        var audience = Environment.GetEnvironmentVariable("AV360_AUDIENCE");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AV360_KEY")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: issuer,audience: audience,
            expires: DateTime.Now.AddMinutes(120),signingCredentials: credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}
