using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Infrastructure.Domclick.DomclickFiltering;

internal sealed class DomclickPublisherFiltering : IParserFilterVisitor
{
    private readonly string _publisher;

    public DomclickPublisherFiltering(string publisher) => _publisher = publisher;

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

    public bool Visit(ParserFilterWithCachedAdvertisements filter) => true;
}
