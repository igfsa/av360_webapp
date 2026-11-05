using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AutenticacaoController(IAutenticacaoService autenticacaoService,
                                    IProfessorNotifier professorNotifier) : ControllerBase
{
    private readonly IAutenticacaoService _autenticacaoService = autenticacaoService;
    private readonly IProfessorNotifier _professorNotifier = professorNotifier;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Register(ProfessorDTO model)
    {
        try
        {
            var professor = await _autenticacaoService.Add(model);

            await _professorNotifier.ProfessorAtualizado(professor.Id);
            return Ok();
        }
        catch
        {
            throw;
        }

    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<string>> Login(LoginDTO login)
    {
        try
        {
            await _autenticacaoService.Login(login.UserName, login.Senha, Response);
            return Ok();
        }
        catch
        {
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            await _autenticacaoService.Refresh(Request, Response);
            return Ok();
        }
        catch
        {
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _autenticacaoService.Logout(Request, Response);
            return Ok();
        }
        catch
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult Me()
    {
        return Ok( new {
            autenticado = true,
            id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            nome = User.FindFirstValue("name"),
            perfil = User.FindFirstValue(ClaimTypes.Role)

        });
    }
}