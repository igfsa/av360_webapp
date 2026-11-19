using Application.DTOs;

namespace Application.Contracts;

public interface ICriterioService
{
    Task<IEnumerable<CriterioDTO>> GetCriterios();
    Task<IEnumerable<CriterioDTO>> GetCriteriosProfessor();
    Task<CriterioDTO> Add(CriterioDTO model);
    Task<CriterioDTO> Update(int criterioId, CriterioDTO model);
    Task<IEnumerable<CriterioDTO>> GetCriteriosTurma(int turmaId);
    Task<IEnumerable<string>> GetCriteriosSugestoes();
}
