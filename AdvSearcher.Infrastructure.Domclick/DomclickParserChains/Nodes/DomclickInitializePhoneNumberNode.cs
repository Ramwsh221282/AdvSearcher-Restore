using System.Text.RegularExpressions;
using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickInitializePhoneNumberNode : IDomclickParserChain
{
    const string phonePattern = @"""phone""\s*:\s*""(?<phone>[^""]+)""";
    const string tokenPattern = @"""token""\s*:\s*""(?<token>[^""]+)""";
    private static readonly Regex phoneRegex = new Regex(phonePattern, RegexOptions.Compiled);
    private static readonly Regex tokenRegex = new Regex(tokenPattern, RegexOptions.Compiled);
    private readonly DomclickParserPipeline _pipeline;
    private readonly DomclickRequestHandler _handler;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickInitializePhoneNumberNode(
        DomclickParserPipeline pipeline,
        DomclickRequestHandler handler,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        _handler = handler;
        Next = next;
    }

    // TODO: First get research api token
    // second get phone number
    public async Task Process()
    {
        if (string.IsNullOrWhiteSpace(_pipeline.CookieFactory.CookieHeaderValue))
            return;
        if (_pipeline.DomclickItems == null || _pipeline.DomclickItems.Length == 0)
            return;
        List<string> phones = [];
        foreach (var item in _pipeline.DomclickItems)
        {
            DomclickResearchApiToken token = await GetResearchApiToken(item);
            string mobilePhone = await GetMobilePhone(item, token);
            phones.Add(mobilePhone);
            item.PhoneNumber = mobilePhone;
        }
        if (Next != null)
            await Next.Process();
    }

    private async Task<DomclickResearchApiToken> GetResearchApiToken(DomclickFetchResult item)
    {
        IHttpRequest request = new DomclickGetResearchApiTokenRequest(
            _pipeline.CookieFactory,
            item
        );
        string response = await _handler.ForceExecuteAsync(request);
        return new DomclickResearchApiToken(ExtractToken(response));
    }

    private async Task<string> GetMobilePhone(
        DomclickFetchResult item,
        DomclickResearchApiToken token
    )
    {
        IHttpRequest request = new DomclickGetPhoneNumberRequest(
            token,
            _pipeline.CookieFactory,
            item
        );
        string response = await _handler.ForceExecuteAsync(request);
        return ExtractPhone(response);
    }

    private static string ExtractPhone(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return string.Empty;
        Match match = phoneRegex.Match(response);
        return match.Success ? match.Groups["phone"].Value : string.Empty;
    }

    private static string ExtractToken(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return string.Empty;
        Match match = tokenRegex.Match(json);
        return match.Success ? match.Groups["token"].Value : string.Empty;
    }
}
