using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains;

internal sealed class DomclickParserPipeline
{
    private readonly List<IParserResponse> _responses = [];
    public IReadOnlyCollection<IParserResponse> Responses => _responses;

    public WebDriverProvider Provider { get; init; }
    public DomclickCookieFactory CookieFactory { get; init; }

    private DomclickFetchResult[]? _domclickItems;
    public DomclickFetchResult[]? DomclickItems => _domclickItems;

    private DomclickResearchApiToken? _researchApiToken;
    public DomclickResearchApiToken? ResearchApiToken => _researchApiToken;

    private string? _qratorValue;
    public string? QratorValue => _qratorValue;

    public DomclickParserPipeline(WebDriverProvider provider)
    {
        Provider = provider;
        CookieFactory = new DomclickCookieFactory(provider);
    }

    public void SetQratorValue(string value)
    {
        if (_qratorValue != null)
            return;
        _qratorValue = value;
    }

    public void SetFetchingResults(DomclickFetchResult[] results)
    {
        if (_domclickItems != null)
            return;
        _domclickItems = results;
    }

    public void InstantiateCookieHeaderValue() => CookieFactory.ExtractCookies();

    public void SetResearchApiToken(DomclickResearchApiToken token)
    {
        if (_researchApiToken != null)
            return;
        _researchApiToken = token;
    }

    public void CleanFromAgents()
    {
        if (_domclickItems == null)
            return;
        _domclickItems = _domclickItems.Where(item => !item.IsAgent).ToArray();
    }

    public void AddParserResponse(IParserResponse response) => _responses.Add(response);
}
