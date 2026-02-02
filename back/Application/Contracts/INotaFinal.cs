using Application.DTOs;

namespace Application.Contracts;

public interface INotaFinalService
{
    Task<NotaFinalDTO> GetById(int id);
    Task<IEnumerable<NotaFinalDTO>> GetNotasFinaisAlunoSessao(int alunoId, int sessaoId); 
    Task<IEnumerable<NotaFinalDTO>> GetNotasFinaisGrupoSessao(int grupoId, int sessaoId); 
    Task<NotaFinalDTO> Add(NotaFinalDTO notaFinal);
    Task<NotaFinalDTO> Update(int NotaFinalId, NotaFinalDTO notaFinal);
}
