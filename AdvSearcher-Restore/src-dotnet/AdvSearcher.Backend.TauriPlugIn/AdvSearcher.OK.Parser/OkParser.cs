using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.OK.Parser.OkParserChains;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.OK.Parser;

public sealed class OkParser : IParser
{
    private IMessageListener? _listener;
    private readonly List<IParserResponse> _results = [];
    private readonly IOkParserChain _chain;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public OkParser(IOkParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url,
        List<ParserFilterOption>? options = null
    )
    {
        _chain.PipeLine.SetServiceUrl(url);
        if (options != null)
        {
            _listener?.Publish("Фильтры применены.");
            _chain.PipeLine.Options = options;
        }
        _listener?.Publish("Начало парсинга ОК.");
        await _chain.ExecuteAsync();
        _results.AddRange(_chain.PipeLine.Responses);
        return true;
    }

    public void SetCurrentProgressValuePublisher(Action<int> actionPublisher) =>
        _chain.PipeLine.CurrentProgressPublisher = actionPublisher;

    public void SetMaxProgressValuePublisher(Action<int> actionPublisher) =>
        _chain.PipeLine.MaxProgressPublisher = actionPublisher;

    public void SetMessageListener(IMessageListener listener) => _listener = listener;

    public void SetNotificationPublisher(Action<string> publisher) =>
        _chain.PipeLine.NotificationsPublisher = publisher;
}
