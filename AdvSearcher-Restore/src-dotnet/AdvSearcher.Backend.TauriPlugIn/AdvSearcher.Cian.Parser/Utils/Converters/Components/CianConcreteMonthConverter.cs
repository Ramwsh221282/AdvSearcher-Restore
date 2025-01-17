using System.Text;

namespace AdvSearcher.Cian.Parser.Utils.Converters.Components;

public sealed class CianConcreteMonthConverter : ICianDateConverterComponent
{
    private static readonly Dictionary<int, string> Months =
        new()
        {
            { 1, "янв" },
            { 2, "фев" },
            { 3, "мар" },
            { 4, "апр" },
            { 5, "май" },
            { 6, "июн" },
            { 7, "июл" },
            { 8, "авг" },
            { 9, "сен" },
            { 10, "окт" },
            { 11, "ноя" },
            { 12, "дек" },
        };

    private readonly int _monthNumber;
    private readonly string _stringDate = string.Empty;

    public CianConcreteMonthConverter(string? stringDate)
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
            foreach (var pattern in Months)
            {
                if (word.Contains(pattern.Value, StringComparison.OrdinalIgnoreCase))
                {
                    CanConvert = true;
                    _monthNumber = pattern.Key;
                    _stringDate = stringDate;
                    break;
                }
            }
        }
    }

    public bool CanConvert { get; }

    public DateOnly Convert()
    {
        if (_monthNumber == 0)
            return DateOnly.MinValue;

        ReadOnlySpan<string> words = _stringDate.Split(' ');
        var stringBuilder = new StringBuilder();
        foreach (var word in words)
        {
            var characters = word.AsSpan();
            foreach (var character in characters)
            {
                if (char.IsDigit(character))
                    stringBuilder.Append(character);
            }
        }
        var days = int.Parse(stringBuilder.ToString());
        var date = new DateTime(DateTime.Now.Year, _monthNumber, days);
        return DateOnly.FromDateTime(date);
    }
}
