namespace Application.DTOs;

public class ResultadoNotaParcialDTO
{
    public int Id { get;  set; }
    public int NotaParcialId { get;  set; }
    public int SessaoResultadoNotaFinalId { get;  set; }
    public ResultadoNotaFinalDTO ResultadoNotaFinal { get;  set; } = null!;
    public int AvaliadoResId { get;  set; }
    public string AvaliadoResNome { get;  set; } = null!;
    public int CriterioResId { get;  set; }
    public string CriterioResNome { get;  set; } = null!;
    public decimal Nota { get;  set; }
}
