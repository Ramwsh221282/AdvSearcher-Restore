using System.Text;

namespace AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

internal sealed class AvitoFromDayConverterComponent : IAvitoDateConverterComponent
{
    private const string DaySample1 = "ден";
    private const string DaySample2 = "дня";
    private const string DaySample3 = "дне";

    public bool CanConvert { get; }

    public AvitoFromDayConverterComponent(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        ReadOnlySpan<string> words = stringDate.Split(' ');
        foreach (var word in words)
        {
            if (word.Contains(DaySample1) || word.Contains(DaySample2) || word.Contains(DaySample3))
            {
                CanConvert = true;
                break;
            }
        }
    }

    public DateOnly Convert(string stringDate)
    {
        StringBuilder numericCharacters = new StringBuilder();
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
        var day = int.Parse(numericCharacters.ToString());
        var date = DateTime.Now.AddDays(-day);
        return DateOnly.FromDateTime(date);
    }
}
