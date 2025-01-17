using AdvSearcher.Avito.Parser.Steps.FourthStep.Converters.Components;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Converters;

public sealed class AvitoDateConverter
{
    private readonly IAvitoDateConverterComponent _component;

    public AvitoDateConverter()
    {
        _component = new AvitoFromMonthConverterComponent();
        _component = new AvitoFromMonthWithYearConverter(_component);
        _component = new AvitoFromWeekConverterComponent(_component);
        _component = new AvitoFromDayConverterComponent(_component);
        _component = new AvitoFromHourConverterComponent(_component);
    }

    public Result<DateOnly> Convert(string? stringDate)
    {
        return string.IsNullOrWhiteSpace(stringDate)
            ? ParserErrors.CantConvertDate
            : _component.Convert(stringDate);
    }
}
