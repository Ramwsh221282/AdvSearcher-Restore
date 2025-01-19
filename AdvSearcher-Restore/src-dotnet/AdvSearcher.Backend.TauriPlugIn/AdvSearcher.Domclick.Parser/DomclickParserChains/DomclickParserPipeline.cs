using AdvSearcher.Domclick.Parser.DomclickFiltering;
using AdvSearcher.Domclick.Parser.InternalModels;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Domclick.Parser.DomclickParserChains;

public sealed class DomclickParserPipeline
{
    public List<DomclickFetchResult> FetchResults { get; private set; } = [];
    private List<IParserResponse> _responses = [];
    public IReadOnlyCollection<IParserResponse> Responses => _responses;
    public List<ParserFilterOption> Options { get; set; } = [];
    public Action<int>? CurrentProgressPublisher { get; set; }
    public Action<int>? MaxProgressPublisher { get; set; }
    public Action<string>? NotificationsPublisher { get; set; }

    public void AddFetchResult(DomclickFetchResult result)
    {
        if (!result.IsAgent)
            FetchResults.Add(result);
    }

    public void FilterByDate()
    {
        List<DomclickFetchResult> catalogueItems = [];
        ParserFilter filter = new ParserFilter(Options);
        foreach (var item in FetchResults)
        {
            IParserFilterVisitor visitor = new DomclickDateOnlyFilter(item.PublishedDate);
            if (filter.IsMatchingFilters(visitor))
                catalogueItems.Add(item);
        }
        FetchResults = catalogueItems;
    }

    public void FilterByCache()
    {
        List<DomclickFetchResult> catalogueItems = [];
        ParserFilter filter = new ParserFilter(Options);
        foreach (var item in FetchResults)
        {
            IParserFilterVisitor visitor = new DomclickCacheOnlyFilter(item.Id);
            if (filter.IsMatchingFilters(visitor))
                catalogueItems.Add(item);
        }
        FetchResults = catalogueItems;
    }

    public void FilterByPublishers()
    {
        List<IParserResponse> responses = [];
        ParserFilter filter = new ParserFilter(Options);
        foreach (var response in _responses)
        {
            IParserFilterVisitor visitor = new DomclickPublisherFiltering(response.Publisher.Info);
            if (filter.IsMatchingFilters(visitor))
                responses.Add(response);
        }
        _responses = responses;
    }

    public void AddParserResponse(IParserResponse response) => _responses.Add(response);
}
