using System.Text.RegularExpressions;

namespace AdvSearcher.MachineLearning.SDK;

public static class MLStringExtensions
{
    public static string FormatToLower(this string text)
    {
        return text.ToLower();
    }

    public static string CleanString(this string text)
    {
        return Regex
            .Replace(
                Regex.Replace(
                    Regex.Replace(text, "['\"][^'\\\"]*['\"]", "", RegexOptions.Compiled),
                    "[^а-яА-Я0-9\\s]",
                    "",
                    RegexOptions.Compiled
                ),
                "\\s+",
                " ",
                RegexOptions.Compiled
            )
            .Trim();
    }
}
