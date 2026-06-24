using Application.DTOs;

namespace Application.Contracts;

public interface IProfessorService
{
    Task<IEnumerable<ProfessorDTO>> GetProfessores();
}
