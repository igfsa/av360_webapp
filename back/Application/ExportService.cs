using ClosedXML.Excel;

using Application.DTOs;
using Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ExportService(ILogger<ExportService> logger) :IExportService
{
    private readonly ILogger _logger = logger;
    public async Task<byte[]> ExportAvaliacaoConsolidada(List<AvaliacaoConsolidadaExportDTO> alunosNotas)
    {
        try
        {
            using var workbook = new XLWorkbook();

            var ws = workbook.Worksheets.Add("Avaliações");

            ws.Cell(1, 1).Value = "Aluno";
            ws.Cell(1, 2).Value = "Média";
            ws.Cell(1, 3).Value = "Avaliou";

            int linha = 2;

            foreach (var item in alunosNotas)
            {
                ws.Cell(linha, 1).Value = item.Aluno;
                ws.Cell(linha, 2).Value = item.Nota;
                ws.Cell(linha, 3).Value = item.Avaliou ? "Sim" : "Não";

                linha++;
            }

            ws.Column(2)
                .Style
                .NumberFormat
                .Format = "0.0";

            ws.Columns().AdjustToContents();

            ws.Row(1).Style.Font.Bold = true;

            ws.RangeUsed().SetAutoFilter();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao exportar de sessão");
            throw;
        }
    }
    public async Task<byte[]> ExportResultadoSessao(ResultadoSessaoReportDTO report)
    {
        try
        {
            using var workbook = new XLWorkbook();

            var resumo = workbook.Worksheets.Add("Resumo");

            resumo.Cell(1, 1).Value = "Tipo";
            resumo.Cell(1, 2).Value = "Processados";
            resumo.Cell(1, 3).Value = "Erros";

            resumo.Cell(2, 1).Value = "Geral";
            resumo.Cell(2, 2).Value = report.TotalGeral;
            resumo.Cell(2, 3).Value = report.TotalGeralErros;

            resumo.Cell(3, 1).Value = "Criterios";
            resumo.Cell(3, 2).Value = report.TotalCriterios;
            resumo.Cell(3, 3).Value = report.ErrosCriterios.Count;

            resumo.Cell(4, 1).Value = "Grupos";
            resumo.Cell(4, 2).Value = report.TotalGrupos;
            resumo.Cell(4, 3).Value = report.ErrosGrupos.Count;

            resumo.Cell(5, 1).Value = "Alunos";
            resumo.Cell(5, 2).Value = report.TotalAlunos;
            resumo.Cell(5, 3).Value = report.ErrosAlunos.Count;

            resumo.Cell(6, 1).Value = "NotasFinais";
            resumo.Cell(6, 2).Value = report.TotalNotasFinais;
            resumo.Cell(6, 3).Value = report.ErrosNotasFinais.Count;

            resumo.Cell(7, 1).Value = "NotasParciais";
            resumo.Cell(7, 2).Value = report.TotalNotasParciais;
            resumo.Cell(7, 3).Value = report.ErrosNotasParciais.Count;

            resumo.Columns().AdjustToContents();

            resumo.Row(1).Style.Font.Bold = true;

            resumo.RangeUsed().SetAutoFilter();

            PreencherAba(
                workbook.Worksheets.Add("Grupos"),
                report.ErrosGrupos);

            PreencherAba(
                workbook.Worksheets.Add("Alunos"),
                report.ErrosAlunos);

            PreencherAba(
                workbook.Worksheets.Add("Critérios"),
                report.ErrosCriterios);

            PreencherAba(
                workbook.Worksheets.Add("Notas Finais"),
                report.ErrosNotasFinais);

            PreencherAba(
                workbook.Worksheets.Add("Notas Parciais"),
                report.ErrosNotasParciais);

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao exportar de sessão");
            throw;
        }

    }
    private static void PreencherAba(
        IXLWorksheet ws,
        List<ResultadoSessaoErrorDTO> erros)
    {
        ws.Cell(1, 1).Value = "Tipo";
        ws.Cell(1, 2).Value = "Nome";
        ws.Cell(1, 3).Value = "Erro";

        int linha = 2;

        foreach (var erro in erros)
        {
            ws.Cell(linha, 1).Value = erro.Tipo;
            ws.Cell(linha, 2).Value = erro.Nome;
            ws.Cell(linha, 3).Value = erro.Erro;

            linha++;
        }

        ws.Columns().AdjustToContents();

        ws.Row(1).Style.Font.Bold = true;

        ws.RangeUsed().SetAutoFilter();
    }
}