using System.Globalization;
using System.Text;
using Ude;

namespace Application.Helpers;

public class CSV
{
    public static string DetectarDelimitador(Stream stream)
    {
        stream.Position = 0;

        using var reader = new StreamReader(stream, leaveOpen: true);
        var headerLine = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(headerLine))
            throw new Exception("Arquivo CSV vazio ou inválido.");

        var countSemicolon = headerLine.Count(c => c == ';');
        var countComma = headerLine.Count(c => c == ',');

        stream.Position = 0;

        return countSemicolon > countComma ? ";" : ",";
    }
}
