using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal User => accessor.HttpContext?.User
        ?? throw new UnauthorizedException($"Usuário não Autorizado");

    public int ProfessorId => 
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
        ? id
        : throw new UnauthorizedException("Usuário não autorizado");

    public bool IsAdmin => User.IsInRole("Admin");
}