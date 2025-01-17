using System.Text;

namespace AdvSearcher.Application.Utils;

public static class StringExtensions
{
    public static int ExtractAllDigitsFromString(this string source)
    {
        StringBuilder onlyDigits = new StringBuilder();
        ReadOnlySpan<char> sourceSpan = source.AsSpan();
        foreach (char digit in sourceSpan)
        {
            if (char.IsDigit(digit))
                onlyDigits.Append(digit);
        }
        return int.Parse(onlyDigits.ToString());
    }
}
