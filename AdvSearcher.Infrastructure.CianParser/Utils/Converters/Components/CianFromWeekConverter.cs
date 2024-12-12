namespace AdvSearcher.Infrastructure.CianParser.Utils.Converters.Components;

internal sealed class CianFromWeekConverter : ICianDateConverterComponent
{
    private const string Week = "недел";

    public bool CanConvert { get; }

    private readonly string _stringDate = string.Empty;

    public CianFromWeekConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        CanConvert = false;
        ReadOnlySpan<string> words = stringDate.Split(' ');
        {
            foreach (var word in words)
            {
                if (word.StartsWith(Week, StringComparison.OrdinalIgnoreCase))
                {
                    CanConvert = true;
                    _stringDate = stringDate;
                    break;
                }
            }
        }
    }

    public DateOnly Convert()
    {
        var number = ExtractWeekNumber(_stringDate);
        var date = DateTime.Now.AddDays(-(number * 7));
        return DateOnly.FromDateTime(date);
    }

    private int ExtractWeekNumber(ReadOnlySpan<char> span)
    {
        foreach (var t in span)
        {
            if (char.IsDigit(t))
                return t - 48;
        }

        return 1;
    }
}
