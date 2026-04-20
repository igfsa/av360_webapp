using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Contracts;
using Application.DTOs;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AutenticacaoController(IAutenticacaoService autenticacaoService) : ControllerBase
{
    private readonly IAutenticacaoService _autenticacaoService = autenticacaoService;

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Register(ProfessorDTO model)
    {
        try
        {
            var professor = await _autenticacaoService.Add(model);
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
        return Ok( new {autenticado = true});
    }
}