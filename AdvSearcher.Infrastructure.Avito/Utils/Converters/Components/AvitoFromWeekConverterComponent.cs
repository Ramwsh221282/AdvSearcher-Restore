using System.Text;

namespace AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

internal sealed class AvitoFromWeekConverterComponent : IAvitoDateConverterComponent
{
    private const string WeekSample = "недел";

    public bool CanConvert { get; }

    public AvitoFromWeekConverterComponent(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        ReadOnlySpan<string> words = stringDate.Split(' ');
        foreach (var word in words)
        {
            if (word.Contains(WeekSample))
            {
                CanConvert = true;
                break;
            }
        }
    }

    public DateOnly Convert(string stringDate)
    {
        var numericCharacters = new StringBuilder();
        ReadOnlySpan<string> words = stringDate.Split(' ');
        foreach (var word in words)
        {
            ReadOnlySpan<char> symbols = word.AsSpan();
            foreach (var symbol in symbols)
            {
                if (char.IsDigit(symbol))
                    numericCharacters.Append(symbol);
            }
        }
        int weeksCount = int.Parse(numericCharacters.ToString());
        var date = DateTime.Now;
        date = date.AddDays(-weeksCount * 7);
        return DateOnly.FromDateTime(date);
    }
}
