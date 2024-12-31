using AdvSearcher.Application.Utils;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters.Components;

internal sealed class AvitoFromMonthConverterComponent : IAvitoDateConverterComponent
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

    private readonly ParserConsoleLogger _logger;

    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromMonthConverterComponent(
        ParserConsoleLogger logger,
        IAvitoDateConverterComponent? next = null
    )
    {
        _logger = logger;
        Next = next;
    }

    public Result<DateOnly> Convert(string stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            _logger.Log($"Can't convert date since string input is empty.");
            return ParserErrors.CantConvertDate;
        }

        if (HasAnyMonthSample(stringDate, out int matchedMonth))
        {
            DateOnly date = GetDateOfMatchedMonth(matchedMonth, stringDate);
            _logger.Log(
                $"Date converted from {stringDate} to {date}. Mode selected: From month conversion"
            );
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
