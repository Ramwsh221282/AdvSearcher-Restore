using AdvSearcher.Core.Entities.ServiceUrls;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using AdvSearcher.Parser.SDK.Contracts;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain;

internal sealed class VkParserPipeLine
{
    private readonly List<IParserResponse> _responses = [];
    public IReadOnlyCollection<IParserResponse> Responses => _responses;

    private ServiceUrl? _serviceUrl;
    public ServiceUrl? ServiceUrl => _serviceUrl;

    private readonly VkOptions _options;
    public VkOptions Options => _options;

    private VkRequestParameters? _parameters;
    public VkRequestParameters? Parameters => _parameters;

    private VkGroupInfo? _groupInfo;
    public VkGroupInfo? GroupInfo => _groupInfo;

    private VkItemsJson? _itemsJson;
    public VkItemsJson? ItemsJson => _itemsJson;

    public VkParserPipeLine(IVkOptionsProvider optionsProvider)
    {
        _options = optionsProvider.Provide();
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
