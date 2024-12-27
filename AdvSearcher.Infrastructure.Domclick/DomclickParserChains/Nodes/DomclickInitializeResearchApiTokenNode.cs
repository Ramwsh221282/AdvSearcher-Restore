using System.Text.RegularExpressions;
using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickInitializeResearchApiTokenNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly IHttpService _httpService;
    private readonly IHttpClient _httpClient;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickInitializeResearchApiTokenNode(
        DomclickParserPipeline pipeline,
        IHttpService httpService,
        IHttpClient httpClient,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        _httpService = httpService;
        _httpClient = httpClient;
        Next = next;
    }

    public async Task Process()
    {
        if (string.IsNullOrWhiteSpace(_pipeline.CookieFactory.CookieHeaderValue))
            return;
        if (_pipeline.DomclickItems == null || _pipeline.DomclickItems.Length == 0)
            return;
        DomclickFetchResult firstItem = _pipeline.DomclickItems[0];
        if (string.IsNullOrWhiteSpace(firstItem.Id))
            return;
        IHttpRequest request = new DomclickGetResearchApiTokenRequest(
            _pipeline.CookieFactory,
            firstItem
        );
        await _httpService.Execute(_httpClient, request);
        string response = _httpService.Result;
        if (string.IsNullOrWhiteSpace(response))
            return;
        string token = ExtractToken(response);
        if (string.IsNullOrWhiteSpace(token))
            return;
        _pipeline.SetResearchApiToken(new DomclickResearchApiToken(token));
        if (Next != null)
            await Next.Process();
    }

    private static string ExtractToken(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return string.Empty;

        string pattern = @"""token""\s*:\s*""(?<token>[^""]+)""";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(json);
        return match.Success ? match.Groups["token"].Value : string.Empty;
    }
}
