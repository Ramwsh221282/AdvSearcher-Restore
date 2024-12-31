using AdvSearcher.Application.Utils;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters.Components;

internal sealed class AvitoFromWeekConverterComponent : IAvitoDateConverterComponent
{
    private const string WeekSample = "недел";
    private readonly ParserConsoleLogger _logger;
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromWeekConverterComponent(
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
            _logger.Log("Can't convert date since string input is empty.");
            return ParserErrors.CantConvertDate;
        }

        if (HasWeekSample(stringDate))
        {
            DateOnly dateOnly = GetDateWithWeekDifference(stringDate);
            _logger.Log(
                $"Date conversion from {stringDate} success. Result: {dateOnly}. Conversion mode: From week"
            );
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
