using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Contracts;

public interface IAutenticacaoService
{
    Task Login(string userName, string senha, HttpResponse response);
    Task Logout(HttpRequest request, HttpResponse response);
    Task<ProfessorDTO> Add(ProfessorDTO model);
    Task Refresh(HttpRequest request, HttpResponse response);
}