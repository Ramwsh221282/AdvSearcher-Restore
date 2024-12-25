using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.Converters.Components;

namespace AdvSearcher.Infrastructure.Avito.Utils.Converters;

internal sealed class AvitoDateConverter : IAdvertisementDateConverter<AvitoParserService>
{
    private readonly IAvitoDateConverterComponent _component;

    public AvitoDateConverter()
    {
        _component = new AvitoFromHourConverterComponent(
            new AvitoFromDayConverterComponent(
                new AvitoFromWeekConverterComponent(new AvitoFromMonthConverter())
            )
        );
    }

    public Result<DateOnly> Convert(string? stringDate)
    {
        return string.IsNullOrWhiteSpace(stringDate)
            ? ParserErrors.CantConvertDate
            : _component.Convert(stringDate);
    }
}
