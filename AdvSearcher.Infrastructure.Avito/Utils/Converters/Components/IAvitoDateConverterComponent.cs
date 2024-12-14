namespace AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

internal interface IAvitoDateConverterComponent
{
    public bool CanConvert { get; }
    public DateOnly Convert(string stringDate);
}
