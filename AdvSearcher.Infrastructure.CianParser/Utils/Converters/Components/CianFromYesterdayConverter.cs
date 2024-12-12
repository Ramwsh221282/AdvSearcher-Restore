namespace AdvSearcher.Infrastructure.CianParser.Utils.Converters.Components;

internal sealed class CianFromYesterdayConverter : ICianDateConverterComponent
{
    public bool CanConvert { get; }

    private const string DaySample = "вчер";

    public CianFromYesterdayConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        if (!stringDate.StartsWith(DaySample, StringComparison.OrdinalIgnoreCase))
        {
            CanConvert = false;
            return;
        }

        CanConvert = true;
    }

    public DateOnly Convert()
    {
        var date = DateTime.Now.AddDays(-1);
        return DateOnly.FromDateTime(date);
    }
}
