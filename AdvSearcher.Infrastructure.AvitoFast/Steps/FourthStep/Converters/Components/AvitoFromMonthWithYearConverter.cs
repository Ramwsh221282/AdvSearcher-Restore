using System.Text.RegularExpressions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters.Components;

internal sealed class AvitoFromMonthWithYearConverter : IAvitoDateConverterComponent
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
    private const string Pattern = @"(\d{1,2})\s+апреля\s+(\d{4})";
    private static readonly Regex RegexPattern = new Regex(Pattern, RegexOptions.Compiled);
    private readonly ParserConsoleLogger _logger;
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromMonthWithYearConverter(
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
            _logger.Log("Date is empty. Conversion stopped.");
            return ParserErrors.CantConvertDate;
        }

        Result<DateOnly> conversion = ConvertDate(stringDate);
        if (conversion.IsSuccess)
        {
            _logger.Log(
                $"Converted date string {stringDate}. Result: {conversion.Value}. Mode: Month and Year"
            );
            return conversion.Value;
        }
        return Next?.Convert(stringDate) ?? ParserErrors.CantConvertDate;
    }

    private Result<DateOnly> ConvertDate(string stringDate)
    {
        Match match = RegexPattern.Match(stringDate);
        if (!match.Success)
            return ParserErrors.CantConvertDate;
        string day = match.Groups[1].Value;
        string month = match.Groups[2].Value;
        int dayNumber = int.Parse(day);
        int year = int.Parse(month);
        ReadOnlySpan<string> words = stringDate.Split(' ');
        foreach (var word in words)
        {
            foreach (var pair in Dates)
            {
                if (word.StartsWith(pair.Value, StringComparison.OrdinalIgnoreCase))
                {
                    int monthNumber = pair.Key;
                    return new DateOnly(year, monthNumber, dayNumber);
                }
            }
        }
        return ParserErrors.CantConvertDate;
    }
}
