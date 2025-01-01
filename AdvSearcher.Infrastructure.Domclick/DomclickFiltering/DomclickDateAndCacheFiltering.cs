using AdvSearcher.Application.Contracts.AdvertisementsCache;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastructure.Domclick.DomclickFiltering;

internal sealed class DomclickDateAndCacheFiltering : IParserFilterVisitor
{
    private readonly DateOnly _date;
    private readonly string _id;

    public DomclickDateAndCacheFiltering(DateOnly date, string id)
    {
        _date = date;
        _id = id;
    }

    public bool Visit(ParserFilterWithDate filter) =>
        _date.BelongsPeriod(filter.StartDate, filter.EndDate);

    public bool Visit(ParserFilterWithIgnoreNames filter) => true;

    public bool Visit(ParserFilterWithCachedAdvertisements filter)
    {
        if (!filter.CachedAdvertisements.Any())
            return true;
        foreach (var cached in filter.CachedAdvertisements)
        {
            string name = cached.GetServiceName();
            if (name != "DOMCLICK")
                continue;
            string cacheId = cached.GetId();
            if (_id == cacheId)
                return false;
        }
        return true;
    }
}
