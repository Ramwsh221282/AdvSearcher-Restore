namespace AdvSearcher.OK.Parser.Utils.Converters.Components;

public sealed class OkDayFromMonthConverter : IOkDateConverter
{
    private static readonly Dictionary<int, string> MonthPatterns =
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

    private readonly int _detectedMonth = DateTime.Now.Year;
    private readonly string _stringDate = string.Empty;

    public OkDayFromMonthConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        ReadOnlySpan<string> parts = stringDate.Split(' ');
        foreach (var part in parts)
        {
            foreach (var pattern in MonthPatterns)
            {
                if (!part.StartsWith(pattern.Value, StringComparison.OrdinalIgnoreCase))
                    continue;
                _detectedMonth = pattern.Key;
                CanConvert = true;
                _stringDate = stringDate;
                break;
            }
        }
    }

    public bool CanConvert { get; }

    public DateOnly Convert()
    {
        var splittedDateString = _stringDate.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var stringDay = splittedDateString[0];
        var day = int.Parse(stringDay);
        var date = new DateTime(DateTime.Now.Year, _detectedMonth, day);
        return DateOnly.FromDateTime(date);
    }
}
