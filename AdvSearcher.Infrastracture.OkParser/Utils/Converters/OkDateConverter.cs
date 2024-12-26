using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastracture.OkParser.Utils.Converters.Components;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Converters;

internal class OkDateConverter : IAdvertisementDateConverter<OkParser>
{
    private readonly List<IOkDateConverter> _converters = [];

    public Result<DateOnly> Convert(string? stringDate)
    {
        _converters.Add(new OkFromTodayConverter(stringDate));
        _converters.Add(new OkFromYesterdayConverter(stringDate));
        _converters.Add(new OkDayFromMonthConverter(stringDate));

        foreach (var converter in _converters.Where(converter => converter.CanConvert))
        {
            return converter.Convert();
        }

        return ParserErrors.CantConvertDate;
    }
}
