using Application.DTOs;

namespace Application.Contracts;

public interface INotaParcialService
{
    Task<NotaParcialDTO> GetById(int Id);
    Task<IEnumerable<NotaParcialDTO>> GetNotaParcialNFinalId(int notaFinalId);
    Task<NotaParcialDTO> Add(NotaParcialDTO notaParcial);
    Task<NotaParcialDTO> Update(int NotaParcialId, NotaParcialDTO notaParcial);    
}