namespace AdvSearcher.Infrastracture.OkParser.Utils.Converters;

internal interface IOkDateConverter
{
    bool CanConvert { get; }
    DateOnly Convert();
}
