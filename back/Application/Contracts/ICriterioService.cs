using Application.DTOs;
using Domain.Entities;

namespace Application.Contracts;

public interface ICriterioService
{
    Task<IEnumerable<CriterioDTO>> GetCriterios();
    Task<CriterioDTO> GetCriterioById(int Id);
    Task<CriterioDTO> Add(CriterioDTO model);
    Task<CriterioDTO> Update(int criterioId, CriterioDTO model);
    Task<IEnumerable<CriterioDTO>> GetCriteriosTurma(int turmaId);
}
