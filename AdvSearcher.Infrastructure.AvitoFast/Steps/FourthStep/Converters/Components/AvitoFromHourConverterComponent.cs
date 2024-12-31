using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters.Components;

internal sealed class AvitoFromHourConverterComponent : IAvitoDateConverterComponent
{
    private readonly ParserConsoleLogger _logger;
    private const string HourSample = "час";
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromHourConverterComponent(
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

        if (HasHourSample(stringDate))
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            _logger.Log($"String input {stringDate}. Conversion mode: From hour. Result: {date}");
            return date;
        }
        return Next?.Convert(stringDate) ?? ParserErrors.CantConvertDate;
    }

    private bool HasHourSample(string stringDate)
    {
        ReadOnlySpan<string> words = stringDate.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (word.StartsWith(HourSample, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
