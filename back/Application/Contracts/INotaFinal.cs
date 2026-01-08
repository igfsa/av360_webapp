using Application.DTOs;

namespace Application.Contracts;

public interface INotaFinalService
{
    Task<IEnumerable<NotaFinalDTO>> GetNotasFinal();
    Task<NotaFinalDTO> GetById(int? Id);
    Task<NotaFinalDTO> Add(NotaFinalDTO notaFinal);
    Task<NotaFinalDTO> Update(NotaFinalDTO notaFinal);
}
