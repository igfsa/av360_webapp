using Application.DTOs;

namespace Application.Contracts;

public interface IAutenticacaoService
{
    Task<string> Login(string userName, string senhaHash);
    Task<ProfessorDTO> Add(ProfessorDTO model);
}