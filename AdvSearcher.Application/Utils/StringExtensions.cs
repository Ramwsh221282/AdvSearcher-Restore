using System.Text;

namespace AdvSearcher.Application.Utils;

public static class StringExtensions
{
    public static int ExtractAllDigitsFromString(this string source)
    {
        StringBuilder onlyDigits = new StringBuilder();
        ReadOnlySpan<char> sourceSpan = source.AsSpan();
        foreach (char character in sourceSpan)
        {
            if (char.IsDigit(character))
                onlyDigits.Append(character);
        }
        return int.Parse(onlyDigits.ToString());
    }
}
