namespace AdvSearcher.OK.Parser.Utils.Converters.Components;

public sealed class OkFromTodayConverter : IOkDateConverter
{
    private const string Word = "сегодня";

    public bool CanConvert { get; }

    public OkFromTodayConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        if (stringDate.Contains(Word, StringComparison.OrdinalIgnoreCase))
            CanConvert = true;
    }

    public DateOnly Convert() => DateOnly.FromDateTime(DateTime.Now);
}
