using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Helpers;

public class JWTToken(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateJWTToken(Professor professor)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = _configuration["Jwt:Key"];

        var expiration = int.Parse(
            _configuration["Jwt:ExpirationMinutes"]!);

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, professor.Id.ToString()),
            new Claim("name", professor.UserName),
            new Claim("role", professor.Perfil.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
