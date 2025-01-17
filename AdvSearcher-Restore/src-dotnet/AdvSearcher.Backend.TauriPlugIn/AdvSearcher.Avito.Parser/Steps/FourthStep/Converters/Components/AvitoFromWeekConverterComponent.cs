using AdvSearcher.Application.Utils;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Converters.Components;

public sealed class AvitoFromWeekConverterComponent : IAvitoDateConverterComponent
{
    private const string WeekSample = "недел";
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromWeekConverterComponent(IAvitoDateConverterComponent? next = null)
    {
        Next = next;
    }

    public Result<DateOnly> Convert(string stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
            return ParserErrors.CantConvertDate;

        if (HasWeekSample(stringDate))
        {
            DateOnly dateOnly = GetDateWithWeekDifference(stringDate);
            return dateOnly;
        }
        return Next?.Convert(stringDate) ?? ParserErrors.CantConvertDate;
    }

    private bool HasWeekSample(string stringDate)
    {
        ReadOnlySpan<string> words = stringDate.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            if (word.StartsWith(WeekSample, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private DateOnly GetDateWithWeekDifference(string stringDate)
    {
        int difference = stringDate.ExtractAllDigitsFromString() * 7;
        DateTime current = DateTime.Now;
        return DateOnly.FromDateTime(current.AddDays(-difference));
    }
}
