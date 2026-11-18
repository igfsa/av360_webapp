using Application.DTOs;

namespace Application.Contracts;

public interface IProfessorService
{
    Task<IEnumerable<ProfessorDTO>> GetProfessores();
    Task<ProfessorDTO> Update(int professorId, ProfessorDTO model);
}
