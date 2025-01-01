using AdvSearcher.Application.Contracts.AdvertisementsCache;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace Advsearcher.Infrastructure.VKParser.Filtering;

internal sealed class VkParserFilterVisitior : IParserFilterVisitor
{
    private readonly IParsedAdvertisement _advertisement;
    private readonly IParsedPublisher _publisher;

    public VkParserFilterVisitior(IParsedAdvertisement advertisement, IParsedPublisher publisher)
    {
        _advertisement = advertisement;
        _publisher = publisher;
    }

    public bool Visit(ParserFilterWithDate dateFilter) => true;

    public bool Visit(ParserFilterWithIgnoreNames ignoreNamesFilter)
    {
        if (!ignoreNamesFilter.IgnoredPublishers.Any())
            return true;
        foreach (var publisher in ignoreNamesFilter.IgnoredPublishers)
        {
            if (!publisher.IsIgnored)
                continue;
            if (_publisher.Info.Contains(publisher.Data.Value))
                return false;
        }
        return true;
    }

    public bool Visit(ParserFilterWithCachedAdvertisements cachedAdvertisementsFilter)
    {
        if (!cachedAdvertisementsFilter.CachedAdvertisements.Any())
            return true;
        foreach (var cached in cachedAdvertisementsFilter.CachedAdvertisements)
        {
            string name = cached.GetServiceName();
            if (name != "VK")
                continue;
            string cacheId = cached.GetId();
            if (_advertisement.Id == cacheId)
                return false;
        }
        return true;
    }
}
