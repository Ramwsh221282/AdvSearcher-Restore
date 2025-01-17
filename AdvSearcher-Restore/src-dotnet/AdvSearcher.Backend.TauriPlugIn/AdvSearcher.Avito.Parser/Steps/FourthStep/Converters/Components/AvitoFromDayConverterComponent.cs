using AdvSearcher.Application.Utils;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Converters.Components;

public sealed class AvitoFromDayConverterComponent : IAvitoDateConverterComponent
{
    private static readonly string[] Samples = ["ден", "дня", "дне"];
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromDayConverterComponent(IAvitoDateConverterComponent? next = null)
    {
        Next = next;
    }

    public Result<DateOnly> Convert(string stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
            return ParserErrors.CantConvertDate;
        if (HasAnySample(stringDate))
        {
            DateOnly date = GetDateWithOneDayDifference(stringDate);
            return date;
        }
        return Next?.Convert(stringDate) ?? ParserErrors.CantConvertDate;
    }

    private bool HasAnySample(string stringDate)
    {
        ReadOnlySpan<string> words = stringDate.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (Samples.Any(sample => word.StartsWith(sample, StringComparison.OrdinalIgnoreCase)))
                return true;
        }
        return false;
    }

    private DateOnly GetDateWithOneDayDifference(string stringDate)
    {
        DateTime current = DateTime.Now;
        int dayDifference = stringDate.ExtractAllDigitsFromString();
        return DateOnly.FromDateTime(current.AddDays(-dayDifference));
    }
}
