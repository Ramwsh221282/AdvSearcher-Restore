using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.VK.Parser.Components.VkParserChain;

namespace AdvSearcher.VK.Parser;

public sealed class VkParser : IParser
{
    private readonly IVkParserNode _chainedNode;
    private readonly List<IParserResponse> _results = [];
    private IMessageListener? _listener;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public VkParser(IVkParserNode chainedNode) => _chainedNode = chainedNode;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url,
        List<ParserFilterOption>? options = null
    )
    {
        if (options != null)
        {
            _listener?.Publish("Параметры фильтра применены");
            _chainedNode.PipeLine.FilterOptions = options;
        }
        _listener?.Publish($"Начало парсинга: {url.Value.Value}");
        _chainedNode.PipeLine.SetServiceUrl(url);
        await _chainedNode.ExecuteAsync();
        _results.AddRange(_chainedNode.PipeLine.Responses);
        _listener?.Publish($"Парсинг {url.Value.Value} завершён");
        return true;
    }

    public void SetCurrentProgressValuePublisher(Action<int> actionPublisher) =>
        _chainedNode.PipeLine.SetCurrentProgressPublisher(actionPublisher);

    public void SetMaxProgressValuePublisher(Action<int> actionPublisher) =>
        _chainedNode.PipeLine.SetMaxProgressPublisher(actionPublisher);

    public void SetMessageListener(IMessageListener listener) => _listener = listener;

    public void SetNotificationPublisher(Action<string> publisher) =>
        _chainedNode.PipeLine.SetNotificationsPublisher(publisher);
}
