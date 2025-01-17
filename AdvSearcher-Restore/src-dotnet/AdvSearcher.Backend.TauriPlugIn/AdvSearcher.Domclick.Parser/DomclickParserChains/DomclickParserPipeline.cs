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

    public void FilterByCacheAndDate()
    {
        List<DomclickFetchResult> filtered = [];
        ParserFilter filter = new ParserFilter(Options);
        foreach (var result in FetchResults)
        {
            IParserFilterVisitor visitor = new DomclickDateAndCacheFiltering(
                result.PublishedDate,
                result.Id
            );
            if (filter.IsMatchingFilters(visitor))
                filtered.Add(result);
        }
        FetchResults = filtered;
    }

    public void FilterByPublishers()
    {
        List<IParserResponse> filtered = [];
        ParserFilter filter = new ParserFilter(Options);
        foreach (var result in _responses)
        {
            IParserFilterVisitor visitor = new DomclickPublisherFiltering(result.Publisher.Info);
            if (filter.IsMatchingFilters(visitor))
                filtered.Add(result);
        }
        _responses = filtered;
    }

    public void AddParserResponse(IParserResponse response) => _responses.Add(response);
}
