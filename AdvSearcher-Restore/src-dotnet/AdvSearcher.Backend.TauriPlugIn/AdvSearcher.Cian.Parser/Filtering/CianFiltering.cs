using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Cian.Parser.Filtering;

public sealed class CianFiltering : IParserFilterVisitor
{
    private readonly IParsedAdvertisement _advertisement;
    private readonly IParsedPublisher _publisher;

    public CianFiltering(IParsedAdvertisement advertisement, IParsedPublisher publisher)
    {
        _advertisement = advertisement;
        _publisher = publisher;
    }

    public bool Visit(ParserFilterWithDate filter) =>
        _advertisement.Date.BelongsPeriod(filter.StartDate, filter.EndDate);

    public bool Visit(ParserFilterWithIgnoreNames filter)
    {
        if (!filter.IgnoredPublishers.Any())
            return true;
        foreach (var publisher in filter.IgnoredPublishers)
        {
            if (!publisher.IsIgnored)
                continue;
            if (_publisher.Info.Contains(publisher.Data.Value))
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
            if (name != "CIAN")
                continue;
            string cacheId = cached.GetId();
            if (_advertisement.Id == cacheId)
                return false;
        }
        return true;
    }
}
