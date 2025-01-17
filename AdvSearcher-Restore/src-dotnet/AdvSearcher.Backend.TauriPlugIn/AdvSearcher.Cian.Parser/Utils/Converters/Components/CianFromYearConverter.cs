namespace AdvSearcher.Cian.Parser.Utils.Converters.Components;

public sealed class CianFromYearConverter : ICianDateConverterComponent
{
    public bool CanConvert { get; }

    private const string YearAgo = "год";
    private const string HalfYearAgo = "полгода";

    private readonly bool _isHalfYearAgo;
    private readonly bool _isYearAgo;

    public CianFromYearConverter(string? stringDate)
    {
        if (string.IsNullOrWhiteSpace(stringDate))
        {
            CanConvert = false;
            return;
        }

        if (stringDate.Contains(YearAgo, StringComparison.OrdinalIgnoreCase))
        {
            CanConvert = true;
            _isYearAgo = true;
            return;
        }

        if (stringDate.Contains(HalfYearAgo, StringComparison.OrdinalIgnoreCase))
        {
            CanConvert = true;
            _isHalfYearAgo = true;
            return;
        }

        CanConvert = false;
    }

    public DateOnly Convert()
    {
        if (_isYearAgo)
        {
            var date = DateTime.Now.AddYears(-1);
            return DateOnly.FromDateTime(date);
        }

        if (_isHalfYearAgo)
        {
            var date = DateTime.Now.AddMonths(-6);
            return DateOnly.FromDateTime(date);
        }

        return DateOnly.FromDateTime(DateTime.Now);
    }
}
