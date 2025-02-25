using AdvSearcher.Application.Utils;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Converters.Components;

public sealed class AvitoFromMonthConverterComponent : IAvitoDateConverterComponent
{
    private static readonly Dictionary<int, string> Dates =
        new()
        {
            { 1, "янв" },
            { 2, "фев" },
            { 3, "мар" },
            { 4, "апр" },
            { 5, "ма" },
            { 6, "июн" },
            { 7, "июл" },
            { 8, "авг" },
            { 9, "сен" },
            { 10, "окт" },
            { 11, "ноя" },
            { 12, "дек" },
        };
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromMonthConverterComponent(IAvitoDateConverterComponent? next = null)
    {
        Next = next;
    }

    public Result<DateOnly> Convert(string stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
            return ParserErrors.CantConvertDate;
        if (HasAnyMonthSample(stringDate, out int matchedMonth))
        {
            DateOnly date = GetDateOfMatchedMonth(matchedMonth, stringDate);
            return date;
        }
        return Next?.Convert(stringDate) ?? ParserErrors.CantConvertDate;
    }

    private bool HasAnyMonthSample(string stringDate, out int matchedMonth)
    {
        matchedMonth = 0;
        ReadOnlySpan<string> words = stringDate.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            foreach (var date in Dates)
            {
                if (word.StartsWith(date.Value, StringComparison.OrdinalIgnoreCase))
                {
                    matchedMonth = date.Key;
                    return true;
                }
            }
        }

        return false;
    }

    private DateOnly GetDateOfMatchedMonth(int matchedMonth, string stringDate)
    {
        int year = DateTime.Now.Year;
        int day = stringDate.Split(' ')[0].ExtractAllDigitsFromString();
        return new DateOnly(year, matchedMonth, day);
    }
}
