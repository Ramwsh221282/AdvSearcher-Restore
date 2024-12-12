namespace AdvSearcher.Infrastructure.CianParser.Utils.Converters;

internal interface ICianDateConverterComponent
{
    public bool CanConvert { get; }
    public DateOnly Convert();
}
