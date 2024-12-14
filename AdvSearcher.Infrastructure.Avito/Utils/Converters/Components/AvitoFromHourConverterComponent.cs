namespace AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

internal sealed class AvitoFromHourConverterComponent : IAvitoDateConverterComponent
{
    private const string HourSample = "час";

    public bool CanConvert { get; }

    public AvitoFromHourConverterComponent(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        ReadOnlySpan<string> words = stringDate.Split(' ');
        foreach (var word in words)
        {
            if (word.Contains(HourSample))
            {
                CanConvert = true;
                break;
            }
        }
    }

    public DateOnly Convert(string stringDate)
    {
        var date = DateTime.Now;
        return DateOnly.FromDateTime(date);
    }
}
