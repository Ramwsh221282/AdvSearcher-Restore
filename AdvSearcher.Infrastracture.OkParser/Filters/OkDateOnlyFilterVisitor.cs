using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastracture.OkParser.Filters;

internal sealed class OkDateOnlyFilterVisitor : IParserFilterVisitor
{
    private readonly DateOnly _date;

    public OkDateOnlyFilterVisitor(DateOnly date) => _date = date;

    public bool Visit(ParserFilterWithDate filter) =>
        _date.BelongsPeriod(filter.StartDate, filter.EndDate);

    public bool Visit(ParserFilterWithIgnoreNames filter) => true;

    public bool Visit(ParserFilterWithCachedAdvertisements filter) => true;
}
