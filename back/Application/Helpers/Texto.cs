using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Ude;

namespace Application.Helpers;

public partial class Texto
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();

    public static string Normalizar(string texto)
    {
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
                _ = sb.Append(c);
        }

        return sb.ToString()
            .Normalize(NormalizationForm.FormC);
    }

    public static Encoding EncodingDetector(Stream stream)
    {
        stream.Position = 0;
        var detector = new CharsetDetector();
        detector.Feed(stream);
        detector.DataEnd();
        stream.Position = 0;
        if (detector.Charset != null)
        {
            return Encoding.GetEncoding(detector.Charset);
        }
        return Encoding.GetEncoding("1252");
    }

    public static string ChaveComparacao(string texto)
    {

        var semAcento = Normalizar(texto);
        var sb = new StringBuilder();
        foreach (var c in semAcento)
            if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                sb.Append(c);
        return MyRegex().Replace(sb.ToString(), " ").Trim();
    }

    public static string LimparParaExibicao(string texto) =>
        MyRegex().Replace(texto.Trim(), " ");

}
