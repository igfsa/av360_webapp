using Application.DTOs;

namespace Application.Contracts;

public interface ITurmaService
{
    Task<IEnumerable<TurmaAdmDTO>> GetTurmasAdm();
    Task<TurmaDTO> GetTurmaById(int Id);
    Task<IEnumerable<TurmaDTO>> GetTurmasProfessorId();
    Task<TurmaDTO> Add(TurmaDTO turma);
    Task<TurmaDTO> AddTurmaCriterio(TurmaCriterioDTO model);
    Task<CsvImportResultDTO> ImportarAlunosAsync(int turmaId, CsvImportRequestDTO dto);
    Task<TurmaDTO> Update(int turmaId, TurmaDTO turma);
    Task<TurmaAdmDTO> AlterarProfessor(int turmaId, int professorId);
    Task<TurmaAdmDTO> RemoverProfessor(int turmaId);
}
