using Application.DTOs;

namespace Application.Contracts;

public interface INotaParcialParcial
{
    Task<IEnumerable<NotaParcialDTO>> GetTurmas();
    Task<NotaParcialDTO> GetById(int? Id);
    Task<NotaParcialDTO> Add(NotaParcialDTO notaParcial);
    Task<NotaParcialDTO> Update(NotaParcialDTO notaParcial);
    
}