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

    public static string Normalizar(string texto){
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        var normalized = texto
            .Trim()
            .ToLowerInvariant()
            .Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var unicodeCategory = char.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString()
                        .Normalize(NormalizationForm.FormC);
    }

    public static Encoding EncodingDetector(Stream stream){
        stream.Position = 0;
        var detector = new CharsetDetector();
        detector.Feed(stream);
        detector.DataEnd();
        stream.Position = 0;
        if (detector.Charset != null){
            return Encoding.GetEncoding(detector.Charset);
        }
        return Encoding.GetEncoding("1252");
}}

