namespace AdvSearcher.Infrastracture.OkParser.Utils.Converters;

public interface IOkDateConverter
{
    bool CanConvert { get; }
    DateOnly Convert();
}
