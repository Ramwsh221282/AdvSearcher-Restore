namespace AdvSearcher.OK.Parser.Utils.Converters.Components;

public sealed class OkFromYesterdayConverter : IOkDateConverter
{
    public bool CanConvert { get; }

    public OkFromYesterdayConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        if (stringDate.Contains("вчера", StringComparison.OrdinalIgnoreCase))
            CanConvert = true;
    }

    public DateOnly Convert()
    {
        var date = DateTime.Now.AddDays(-1);
        return DateOnly.FromDateTime(date);
    }
}
