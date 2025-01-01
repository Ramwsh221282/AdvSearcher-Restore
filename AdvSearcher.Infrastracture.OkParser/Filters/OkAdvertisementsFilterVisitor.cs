using AdvSearcher.Application.Contracts.AdvertisementsCache;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastracture.OkParser.Filters;

internal sealed class OkAdvertisementsFilterVisitor : IParserFilterVisitor
{
    private readonly string _publisher;
    private readonly string _id;

    public OkAdvertisementsFilterVisitor(string publisher, string id)
    {
        _publisher = publisher;
        _id = id;
    }

    public bool Visit(ParserFilterWithDate filter) => true;

    public bool Visit(ParserFilterWithIgnoreNames filter)
    {
        if (!filter.IgnoredPublishers.Any())
            return true;
        foreach (var publisher in filter.IgnoredPublishers)
        {
            if (!publisher.IsIgnored)
                continue;
            if (_publisher.Contains(publisher.Data.Value))
                return false;
        }
        return true;
    }

    public bool Visit(ParserFilterWithCachedAdvertisements filter)
    {
        if (!filter.CachedAdvertisements.Any())
            return true;
        foreach (var cached in filter.CachedAdvertisements)
        {
            string name = cached.GetServiceName();
            if (name != "OK")
                continue;
            string cacheId = cached.GetId();
            if (_id == cacheId)
                return false;
        }
        return true;
    }
}
