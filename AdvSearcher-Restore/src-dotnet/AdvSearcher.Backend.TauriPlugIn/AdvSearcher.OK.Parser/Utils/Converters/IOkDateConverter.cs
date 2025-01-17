namespace AdvSearcher.OK.Parser.Utils.Converters;

public interface IOkDateConverter
{
    bool CanConvert { get; }
    DateOnly Convert();
}
