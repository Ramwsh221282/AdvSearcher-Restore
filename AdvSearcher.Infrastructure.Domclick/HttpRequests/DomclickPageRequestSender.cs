using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Queries;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickPageRequestSender
{
    private readonly IDomclickFetchingResultFactory _factory;
    private readonly DomclickParserPipeline _pipeline;
    private readonly WebDriverProvider _provider;
    private int _offset;
    private int _maxCount;
    private readonly List<DomclickFetchResult> _results = [];

    public IReadOnlyCollection<DomclickFetchResult> Results => _results;

    public DomclickPageRequestSender(
        IDomclickFetchingResultFactory factory,
        DomclickParserPipeline pipeline,
        WebDriverProvider provider
    )
    {
        _factory = factory;
        _pipeline = pipeline;
        _provider = provider;
    }

    public async Task ConstructFetchResults()
    {
        _provider.InstantiateNewWebDriver();
        bool IsNotFirstFetch = true;
        while (true)
        {
            if (this._offset + 20 < this._maxCount || IsNotFirstFetch)
            {
                string url = CreateUrl();
                await new NavigateOnPageCommand(url).ExecuteAsync(_provider);
                string response = await new DomclickGetCatalogueItemsResponseQuery().ExecuteAsync(
                    _provider
                );
                if (string.IsNullOrEmpty(response))
                    break;
                UpdateMaxCount(response);
                UpdateOffset();
                ConstructFetchResultsFromResponse(response);
                if (IsNotFirstFetch)
                    IsNotFirstFetch = false;
            }
            else
                break;
        }
        FillResults();
        _provider.Dispose();
    }

    private string CreateUrl()
    {
        string url =
            $"https://bff-search-web.domclick.ru/api/offers/v1?address=6b2a4aad-bd39-4982-9ee0-2cc25449964b&offset={_offset}&limit=20&sort=qi&sort_dir=desc&deal_type=sale&category=living&offer_type=flat&offer_type=layout&aids=650885&sort_by_tariff_date=1";
        return url;
    }

    private void UpdateOffset() => _offset += 20;

    private void UpdateMaxCount(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return;
        if (this._maxCount != 0)
            return;
        JObject jsonObject = JObject.Parse(response);
        JToken? result = jsonObject["result"];
        if (result == null)
            return;
        JToken? pagination = result["pagination"];
        if (pagination == null)
            return;
        JToken? total = pagination["total"];
        if (total == null)
            return;
        _maxCount = int.Parse(total.ToString());
    }

    private void ConstructFetchResultsFromResponse(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return;
        _results.AddRange(_factory.Create(response));
    }

    private void FillResults()
    {
        foreach (var result in _results)
        {
            _pipeline.AddFetchResult(result);
        }
    }
}
