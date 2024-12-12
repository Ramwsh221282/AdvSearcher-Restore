namespace AdvSearcher.Infrastructure.CianParser.Utils.Converters.Components;

internal sealed class CianFromMonthConverter : ICianDateConverterComponent
{
    private const string Month = "меся";

    public bool CanConvert { get; }

    private readonly string _stringDate = string.Empty;

    public CianFromMonthConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        CanConvert = false;
        ReadOnlySpan<string> words = stringDate.Split(' ');
        foreach (var word in words)
        {
            if (word.StartsWith(Month, StringComparison.OrdinalIgnoreCase))
            {
                CanConvert = true;
                _stringDate = stringDate;
                break;
            }
        }
    }

    public DateOnly Convert()
    {
        var span = _stringDate.AsSpan();
        foreach (var character in span)
        {
            if (!char.IsDigit(character))
                continue;
            var month = character - 48;
            var date = DateTime.Now.AddMonths(-month);
            return DateOnly.FromDateTime(date);
        }
        return DateOnly.FromDateTime(DateTime.Now);
    }
}
