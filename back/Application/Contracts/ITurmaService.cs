using Application.DTOs;

namespace Application.Contracts;

public interface ITurmaService
{
    Task<IEnumerable<TurmaDTO>> GetTurmas();
    Task<TurmaDTO> GetTurmaById(int Id);
    Task<TurmaDTO> Add(TurmaDTO turma);
    Task<TurmaDTO> AddTurmaCriterio(TurmaCriterioDTO model);
    Task<CsvImportResultDTO> ImportarAlunosAsync(int turmaId, CsvImportRequestDTO dto);
    Task<TurmaDTO> Update(int turmaId, TurmaDTO turma);
}
