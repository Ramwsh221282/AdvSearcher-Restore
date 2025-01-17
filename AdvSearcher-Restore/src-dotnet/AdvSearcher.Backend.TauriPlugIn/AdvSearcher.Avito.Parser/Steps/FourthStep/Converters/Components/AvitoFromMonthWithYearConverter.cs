using System.Text.RegularExpressions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Converters.Components;

public sealed class AvitoFromMonthWithYearConverter : IAvitoDateConverterComponent
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
    private const string Pattern = @"(\d{1,2})\s+([\p{L}]+)\s+(\d{4})";
    private static readonly Regex RegexPattern = new Regex(Pattern, RegexOptions.Compiled);
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromMonthWithYearConverter(IAvitoDateConverterComponent? next = null)
    {
        Next = next;
    }

    public Result<DateOnly> Convert(string stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
            return ParserErrors.CantConvertDate;
        Result<DateOnly> conversion = ConvertDate(stringDate);
        if (conversion.IsSuccess)
            return conversion.Value;
        return Next?.Convert(stringDate) ?? ParserErrors.CantConvertDate;
    }

    private Result<DateOnly> ConvertDate(string stringDate)
    {
        Match match = RegexPattern.Match(stringDate);
        if (!match.Success)
            return ParserErrors.CantConvertDate;
        string dayString = match.Groups[1].Value;
        string monthString = match.Groups[2].Value;
        string yearString = match.Groups[3].Value;
        int yearNumber = int.Parse(yearString);
        var monthPair = Dates.FirstOrDefault(d =>
            monthString.StartsWith(d.Value, StringComparison.OrdinalIgnoreCase)
        );
        int monthNumber = monthPair.Key;
        int dayNumber = int.Parse(dayString);
        return new DateOnly(yearNumber, monthNumber, dayNumber);
    }
}
