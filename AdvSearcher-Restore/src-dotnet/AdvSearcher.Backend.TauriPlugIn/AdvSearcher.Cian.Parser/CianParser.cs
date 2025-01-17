using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Cian.Parser.CianParserChains;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Cian.Parser;

public sealed class CianParser : IParser
{
    private IMessageListener? _listener;
    private readonly List<IParserResponse> _results = [];
    private readonly ICianParserChain _chain;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public CianParser(ICianParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url,
        List<ParserFilterOption>? options = null
    )
    {
        _chain.PipeLine.SetServiceUrl(url);
        if (options != null)
        {
            _listener?.Publish("Фильтры применены");
            _chain.PipeLine.Options = options;
        }
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
