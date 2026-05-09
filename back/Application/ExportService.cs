using ClosedXML.Excel;

using Application.DTOs;
using Application.Contracts;

namespace Application.Services;

public class ExportService() :IExportService
{
    public async Task<byte[]> ExportAvaliacaoConsolidada(List<AvaliacaoConsolidadaExportDTO> alunosNotas)
    {
        try
        {
            using var workbook = new XLWorkbook();

            var ws = workbook.Worksheets.Add("Avaliações");

            ws.Cell(1, 1).Value = "Aluno";
            ws.Cell(1, 2).Value = "Média";

            int linha = 2;

            foreach (var item in alunosNotas)
            {
                ws.Cell(linha, 1).Value = item.Aluno;
                ws.Cell(linha, 2).Value = item.Nota;

                linha++;
            }

            ws.Column(2)
                .Style
                .NumberFormat
                .Format = "0.00";

            ws.Columns().AdjustToContents();

            ws.Row(1).Style.Font.Bold = true;

            ws.RangeUsed().SetAutoFilter();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }
        catch
        {
            throw;
        }
    }
}