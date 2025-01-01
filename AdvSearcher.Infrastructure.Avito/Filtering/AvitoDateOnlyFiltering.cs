using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastructure.Avito.Filtering;

internal sealed class AvitoDateOnlyFiltering : IParserFilterVisitor
{
    private readonly DateOnly _date;

    public AvitoDateOnlyFiltering(DateOnly date) => _date = date;

    public bool Visit(ParserFilterWithDate filter) =>
        _date.BelongsPeriod(filter.StartDate, filter.EndDate);

    public bool Visit(ParserFilterWithIgnoreNames filter) => true;

    public bool Visit(ParserFilterWithCachedAdvertisements filter) => true;
}
