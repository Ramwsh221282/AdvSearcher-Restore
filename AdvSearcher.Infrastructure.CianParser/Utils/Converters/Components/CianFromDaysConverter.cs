namespace AdvSearcher.Infrastructure.CianParser.Utils.Converters.Components;

internal sealed class CianFromDaysConverter : ICianDateConverterComponent
{
    private static readonly Dictionary<int, string> CianDayPatterns =
        new()
        {
            { 2, "дв" },
            { 3, "тр" },
            { 4, "четыр" },
            { 5, "пят" },
            { 6, "шест" },
        };

    private const string DaySample1 = "дне";
    private const string DaySample2 = "дн";

    public bool CanConvert { get; }

    private readonly string _stringDate = string.Empty;

    public CianFromDaysConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        CanConvert = false;
        ReadOnlySpan<string> splittedWords = stringDate.Split(' ');
        foreach (var word in splittedWords)
        {
            if (
                word.StartsWith(DaySample1, StringComparison.OrdinalIgnoreCase)
                || word.StartsWith(DaySample2, StringComparison.OrdinalIgnoreCase)
            )
            {
                CanConvert = true;
                _stringDate = stringDate;
                break;
            }
        }
    }

    public DateOnly Convert()
    {
        foreach (var dayPattern in CianDayPatterns)
        {
            ReadOnlySpan<string> splittedWords = _stringDate.Split(' ');
            foreach (var word in splittedWords)
            {
                if (word.StartsWith(dayPattern.Value, StringComparison.OrdinalIgnoreCase))
                {
                    var date = DateTime.Now.AddDays(-dayPattern.Key);
                    return DateOnly.FromDateTime(date);
                }
            }
        }
        return DateOnly.MinValue;
    }
}
