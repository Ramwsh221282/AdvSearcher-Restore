using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Domclick.Parser.DomclickFiltering;

public sealed class DomclickCacheOnlyFilter : IParserFilterVisitor
{
    private readonly string _id;

    public DomclickCacheOnlyFilter(string id) => _id = id;

    public bool Visit(ParserFilterWithDate filter) => true;

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
