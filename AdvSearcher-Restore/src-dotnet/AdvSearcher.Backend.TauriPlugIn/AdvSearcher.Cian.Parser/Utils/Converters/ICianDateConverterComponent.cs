namespace AdvSearcher.Cian.Parser.Utils.Converters;

public interface ICianDateConverterComponent
{
    public bool CanConvert { get; }
    public DateOnly Convert();
}
