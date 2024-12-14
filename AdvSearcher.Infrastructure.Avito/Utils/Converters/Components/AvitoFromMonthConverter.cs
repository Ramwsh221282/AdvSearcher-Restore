namespace AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

internal sealed class AvitoFromMonthConverter : IAvitoDateConverterComponent
{
    private static readonly Dictionary<int, string> Dates =
        new()
        {
            { 1, "янв" },
            { 2, "фев" },
            { 3, "мар" },
            { 4, "апр" },
            { 5, "ма" },
            { 6, "июн" },
            { 7, "июл" },
            { 8, "авг" },
            { 9, "сен" },
            { 10, "окт" },
            { 11, "ноя" },
            { 12, "дек" },
        };

    private readonly int _matchedMonth = 0;

    public bool CanConvert { get; }

    public AvitoFromMonthConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        ReadOnlySpan<string> words = stringDate.Split(' ');
        foreach (var word in words)
        {
            foreach (var pattern in Dates)
            {
                if (word.Contains(pattern.Value))
                {
                    _matchedMonth = pattern.Key;
                    CanConvert = true;
                    return;
                }
            }
        }
    }

    public DateOnly Convert(string stringDate)
    {
        var date = DateTime.Now;
        var year = date.Year;
        var day = date.Day;
        var result = new DateTime(year, _matchedMonth, day);
        return DateOnly.FromDateTime(result);
    }
}
