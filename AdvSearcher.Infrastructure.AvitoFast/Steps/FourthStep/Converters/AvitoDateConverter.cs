using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters.Components;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Converters;

internal sealed class AvitoDateConverter
{
    private readonly IAvitoDateConverterComponent _component;

    public AvitoDateConverter(ParserConsoleLogger logger)
    {
        _component = new AvitoFromMonthConverterComponent(logger);
        _component = new AvitoFromMonthWithYearConverter(logger, _component);
        _component = new AvitoFromWeekConverterComponent(logger, _component);
        _component = new AvitoFromDayConverterComponent(logger, _component);
        _component = new AvitoFromHourConverterComponent(logger, _component);

        // _component = new AvitoFromHourConverterComponent(
        //     logger,
        //     new AvitoFromDayConverterComponent(
        //         logger,
        //         new AvitoFromWeekConverterComponent(
        //             logger,
        //             new AvitoFromMonthConverterComponent(logger)
        //         )
        //     )
        // );
    }

    public Result<DateOnly> Convert(string? stringDate)
    {
        return string.IsNullOrWhiteSpace(stringDate)
            ? ParserErrors.CantConvertDate
            : _component.Convert(stringDate);
    }
}
