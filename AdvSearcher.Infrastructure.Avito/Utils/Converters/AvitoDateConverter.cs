using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

namespace AdvSearcher.Infrastructure.Avito.Utils.Converters;

internal sealed class AvitoDateConverter : IAdvertisementDateConverter<AvitoParser>
{
    private readonly List<IAvitoDateConverterComponent> _components = [];

    public Result<DateOnly> Convert(string? stringDate)
    {
        FillComponentsIfEmpty(stringDate);
        foreach (var component in _components)
        {
            if (component.CanConvert)
                return component.Convert(stringDate!);
        }

        return ParserErrors.CantConvertDate;
    }

    private void FillComponentsIfEmpty(string? stringDate)
    {
        if (_components.Count != 0)
            return;

        _components.Add(new AvitoFromDayConverterComponent(stringDate));
        _components.Add(new AvitoFromHourConverterComponent(stringDate));
        _components.Add(new AvitoFromWeekConverterComponent(stringDate));
        _components.Add(new AvitoFromMonthConverter(stringDate));
    }
}
