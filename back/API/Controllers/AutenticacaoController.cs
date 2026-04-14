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
    [HttpPost("")]
    [ActionName("Register")]
    public async Task<ActionResult<ProfessorDTO>> Register(ProfessorDTO model)
    {
        try
        {
            var professor = await _autenticacaoService.Add(model);
            return Ok(professor);
        }
        catch
        {
            throw;
        }

    }

    [AllowAnonymous]
    [HttpPost("")]
    [ActionName("Login")]
    public async Task<ActionResult<string>> Login(LoginDTO login)
    {
        try
        {
            var token = await _autenticacaoService.Login(login.UserName, login.Senha);
            return Ok(new {token});
        }
        catch
        {
            throw;
        }
    }
}