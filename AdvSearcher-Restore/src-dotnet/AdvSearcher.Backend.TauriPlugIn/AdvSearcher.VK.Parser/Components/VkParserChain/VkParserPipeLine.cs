using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.VK.Parser.Components.Responses;

namespace AdvSearcher.VK.Parser.Components.VkParserChain;

public sealed class VkParserPipeLine
{
    private readonly List<IParserResponse> _responses = [];
    private readonly IVKOptionsProvider _optionsProvider;
    public IReadOnlyCollection<IParserResponse> Responses => _responses;

    private ServiceUrl? _serviceUrl;
    public ServiceUrl? ServiceUrl => _serviceUrl;

    private VKOptions? _options;
    public VKOptions? Options => _options;

    private VkRequestParameters? _parameters;
    public VkRequestParameters? Parameters => _parameters;

    private VkGroupInfo? _groupInfo;
    public VkGroupInfo? GroupInfo => _groupInfo;

    private VkItemsJson? _itemsJson;
    public VkItemsJson? ItemsJson => _itemsJson;

    public Action<int>? CurrentProgressPublisher { get; private set; }
    public Action<int>? MaxProgressPublisher { get; private set; }
    public Action<string>? NotificationsPublisher { get; private set; }

    public List<ParserFilterOption> FilterOptions { get; set; } = [];

    public bool AreTokensCorrect =>
        _options != null
        && !string.IsNullOrWhiteSpace(_options.ServiceAccessToken)
        && !string.IsNullOrWhiteSpace(_options.OAuthAccessToken);

    public VkParserPipeLine(IVKOptionsProvider optionsProvider)
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

    public void SetCurrentProgressPublisher(Action<int> publisher)
    {
        if (CurrentProgressPublisher != null)
            return;
        CurrentProgressPublisher = publisher;
    }

    public void SetMaxProgressPublisher(Action<int> publisher)
    {
        if (MaxProgressPublisher != null)
            return;
        MaxProgressPublisher = publisher;
    }

    public void SetNotificationsPublisher(Action<string> publisher)
    {
        if (NotificationsPublisher != null)
            return;
        NotificationsPublisher = publisher;
    }

    public void PublishMaxProgressValue(int value) => MaxProgressPublisher?.Invoke(value);

    public void PublishCurrentProgressValue(int value) => CurrentProgressPublisher?.Invoke(value);

    public void AddResponse(IParserResponse response) => _responses.Add(response);
}
