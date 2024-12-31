using AdvSearcher.Application.Utils;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters.Components;

internal sealed class AvitoFromDayConverterComponent : IAvitoDateConverterComponent
{
    private static readonly string[] Samples = ["ден", "дня", "дне"];
    private readonly ParserConsoleLogger _logger;

    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromDayConverterComponent(
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
            _logger.Log("Can't convert date. String input is empty.");
            return ParserErrors.CantConvertDate;
        }

        if (HasAnySample(stringDate))
        {
            DateOnly date = GetDateWithOneDayDifference(stringDate);
            _logger.Log($"Date: {stringDate}. Conversion mode: From day. Converted: {date}.");
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
