using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Domclick.Parser.DomclickFiltering;

public sealed class DomclickDateOnlyFilter : IParserFilterVisitor
{
    private readonly DateOnly _date;

    public DomclickDateOnlyFilter(DateOnly date) => _date = date;

    public bool Visit(ParserFilterWithDate filter) =>
        _date.BelongsPeriod(filter.StartDate, filter.EndDate);

    public bool Visit(ParserFilterWithIgnoreNames filter) => true;

    public bool Visit(ParserFilterWithCachedAdvertisements filter) => true;
}
