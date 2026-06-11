using Domain.Entities;

namespace Application.DTOs;

public class ResultadoSessaoDTO
{
    public int Id { get; set; }
    public int SessaoId { get; set; }
    public SessaoDTO Sessao { get; set; } = null!;
    public int TurmaId { get; set; }
    public string TurmaCod { get; set; } = null!;
    public decimal NotaMaxima { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    private readonly List<ResultadoNotaFinalDTO> _notasFinais = [];
    public IReadOnlyCollection<ResultadoNotaFinalDTO> NotasFinais => _notasFinais;
    private readonly List<ResultadoGrupoDTO> _grupos = [];
    public IReadOnlyCollection<ResultadoGrupoDTO> Grupos => _grupos;    
    private readonly List<ResultadoAlunoDTO> _alunos = [];
    public IReadOnlyCollection<ResultadoAlunoDTO> Alunos => _alunos;
    private readonly List<ResultadoCriterioDTO> _criterios = [];
    public IReadOnlyCollection<ResultadoCriterioDTO> Criterios => _criterios;
}