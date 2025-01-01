using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastructure.AvitoFast.Filtering;

internal sealed class AvitoFastDateOnlyFiltering : IParserFilterVisitor
{
    private readonly DateOnly _date;

    public AvitoFastDateOnlyFiltering(DateOnly date) => _date = date;

    public bool Visit(ParserFilterWithDate filter) =>
        _date.BelongsPeriod(filter.StartDate, filter.EndDate);

    public bool Visit(ParserFilterWithIgnoreNames filter) => true;

    public bool Visit(ParserFilterWithCachedAdvertisements filter) => true;
}
