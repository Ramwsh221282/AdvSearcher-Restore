using AdvSearcher.Application.Contracts.AdvertisementsCache;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastructure.AvitoFast.Filtering;

internal sealed class AvitoFastCacheOnlyFiltering : IParserFilterVisitor
{
    private readonly string _id;

    public AvitoFastCacheOnlyFiltering(string id) => _id = id;

    public bool Visit(ParserFilterWithDate filter) => true;

    public bool Visit(ParserFilterWithIgnoreNames filter) => true;

    public bool Visit(ParserFilterWithCachedAdvertisements filter)
    {
        if (!filter.CachedAdvertisements.Any())
            return true;
        foreach (var cached in filter.CachedAdvertisements)
        {
            string name = cached.GetServiceName();
            if (name != "AVITO")
                continue;
            string cacheId = cached.GetId();
            if (_id == cacheId)
                return false;
        }
        return true;
    }
}
