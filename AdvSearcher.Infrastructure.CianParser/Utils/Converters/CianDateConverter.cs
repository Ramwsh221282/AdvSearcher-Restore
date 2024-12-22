using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.CianParser.Utils.Converters.Components;

namespace AdvSearcher.Infrastructure.CianParser.Utils.Converters;

internal sealed class CianDateConverter : IAdvertisementDateConverter<CianParser>
{
    private readonly List<ICianDateConverterComponent> _components = [];

    public Result<DateOnly> Convert(string? stringDate)
    {
        FillComponentsIfEmpty(stringDate);
        foreach (var component in _components)
        {
            if (component.CanConvert)
                return component.Convert();
        }
        return ParserErrors.CantConvertDate;
    }

    private void FillComponentsIfEmpty(string? stringDate)
    {
        if (_components.Count > 0)
            return;
        _components.Add(new CianConcreteMonthConverter(stringDate));
        _components.Add(new CianFromYesterdayConverter(stringDate));
        _components.Add(new CianFromYearConverter(stringDate));
        _components.Add(new CianFromMonthConverter(stringDate));
        _components.Add(new CianFromWeekConverter(stringDate));
        _components.Add(new CianFromDaysConverter(stringDate));
        _components.Add(new CianFromCurrentDateConverter(stringDate));
    }
}
