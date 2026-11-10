using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Contracts;

public interface IAutenticacaoService
{
    Task<MeDTO> Me();
    Task Login(string userName, string senha, HttpResponse response);
    Task Logout(HttpRequest request, HttpResponse response);
    Task<ProfessorDTO> Add(ProfessorRegisterDTO model);
    Task Refresh(HttpRequest request, HttpResponse response);
}