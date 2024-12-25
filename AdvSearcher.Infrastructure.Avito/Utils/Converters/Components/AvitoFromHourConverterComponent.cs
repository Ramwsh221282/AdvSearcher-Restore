using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

internal sealed class AvitoFromHourConverterComponent : IAvitoDateConverterComponent
{
    private const string HourSample = "час";
    public IAvitoDateConverterComponent? Next { get; }

    public AvitoFromHourConverterComponent(IAvitoDateConverterComponent? next = null) =>
        Next = next;

    public Result<DateOnly> Convert(string stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
            return ParserErrors.CantConvertDate;
        if (HasHourSample(stringDate))
            return DateOnly.FromDateTime(DateTime.Now);
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
