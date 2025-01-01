using AdvSearcher.Core.Entities.ServiceUrls;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain;

internal sealed class VkParserPipeLine
{
    private readonly List<IParserResponse> _responses = [];
    private readonly IVkOptionsProvider _optionsProvider;
    public IReadOnlyCollection<IParserResponse> Responses => _responses;

    private ServiceUrl? _serviceUrl;
    public ServiceUrl? ServiceUrl => _serviceUrl;

    private VkOptions? _options;
    public VkOptions? Options => _options;

    private VkRequestParameters? _parameters;
    public VkRequestParameters? Parameters => _parameters;

    private VkGroupInfo? _groupInfo;
    public VkGroupInfo? GroupInfo => _groupInfo;

    private VkItemsJson? _itemsJson;
    public VkItemsJson? ItemsJson => _itemsJson;

    public List<ParserFilterOption> FilterOptions { get; set; } = [];

    public bool AreTokensCorrect =>
        _options != null
        && !string.IsNullOrWhiteSpace(_options.ServiceAccessToken)
        && !string.IsNullOrWhiteSpace(_options.OAuthAccessToken);

    public VkParserPipeLine(IVkOptionsProvider optionsProvider)
    {
        _optionsProvider = optionsProvider;
    }

    public void InstantiateOptions()
    {
        if (_options != null)
            return;
        _options = _optionsProvider.Provide();
    }

    public void SetParameters(VkRequestParameters parameters)
    {
        if (_parameters != null)
            return;
        _parameters = parameters;
    }

    public void SetGroupInfo(VkGroupInfo groupInfo)
    {
        if (_groupInfo != null)
            return;
        _groupInfo = groupInfo;
    }

    public void SetItemsJson(VkItemsJson itemsJson)
    {
        if (_itemsJson != null)
            return;
        _itemsJson = itemsJson;
    }

    public void SetServiceUrl(ServiceUrl serviceUrl)
    {
        if (_serviceUrl != null)
            return;
        _serviceUrl = serviceUrl;
    }

    public void AddResponse(IParserResponse response) => _responses.Add(response);
}
