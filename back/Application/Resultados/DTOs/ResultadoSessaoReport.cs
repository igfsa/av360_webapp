namespace Application.DTOs;

public class ResultadoSessaoReportDTO
{
    public int TotalGeral { get; set; }
    public int TotalGeralErros =>
        ErrosAlunos.Count +
        ErrosCriterios.Count +
        ErrosGrupos.Count +
        ErrosNotasFinais.Count +
        ErrosNotasParciais.Count;
    public int TotalGrupos { get; set; }
    public List<ResultadoSessaoErrorDTO> ErrosGrupos { get; set; } = [];
    public int TotalAlunos { get; set; }
    public List<ResultadoSessaoErrorDTO> ErrosAlunos { get; set; } = [];
    public int TotalCriterios { get; set; }
    public List<ResultadoSessaoErrorDTO> ErrosCriterios { get; set; } = [];
    public int TotalNotasFinais { get; set; }
    public List<ResultadoSessaoErrorDTO> ErrosNotasFinais { get; set; } = [];
    public int TotalNotasParciais { get; set; }
    public List<ResultadoSessaoErrorDTO> ErrosNotasParciais { get; set; } = [];
}