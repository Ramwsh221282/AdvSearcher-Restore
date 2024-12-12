namespace AdvSearcher.Infrastructure.CianParser.Utils.Converters.Components;

internal sealed class CianFromCurrentDateConverter : ICianDateConverterComponent
{
    private const string Today = "сегод";
    private const string Hour = "час";
    private const string Minute = "мину";

    public bool CanConvert { get; }

    public CianFromCurrentDateConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        ReadOnlySpan<string> words = stringDate.Split(' ');
        CanConvert = false;
        foreach (var word in words)
        {
            if (
                word.StartsWith(Today, StringComparison.OrdinalIgnoreCase)
                || word.StartsWith(Hour, StringComparison.OrdinalIgnoreCase)
                || word.StartsWith(Minute, StringComparison.OrdinalIgnoreCase)
            )
            {
                CanConvert = true;
                break;
            }
        }
    }

    public DateOnly Convert()
    {
        var date = DateTime.Now;
        return DateOnly.FromDateTime(date);
    }
}
