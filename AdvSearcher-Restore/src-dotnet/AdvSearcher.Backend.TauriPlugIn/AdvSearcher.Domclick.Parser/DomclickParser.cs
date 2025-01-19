using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Domclick.Parser.DomclickParserChains;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Domclick.Parser;

public sealed class DomclickParser : IParser
{
    private readonly IDomclickParserChain _chain;
    private readonly List<IParserResponse> _results = [];
    private IMessageListener? _listener;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public DomclickParser(IDomclickParserChain chain) => _chain = chain;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url = null!,
        List<ParserFilterOption>? options = null
    )
    {
        if (options != null)
        {
            _listener?.Publish("Фильтры применены.");
            _chain.Pipeline.Options = options;
        }
        await _chain.Process();
        _results.AddRange(_chain.Pipeline.Responses);
        return true;
    }

    public void SetCurrentProgressValuePublisher(Action<int> actionPublisher) =>
        _chain.Pipeline.CurrentProgressPublisher = actionPublisher;

    public void SetMaxProgressValuePublisher(Action<int> actionPublisher) =>
        _chain.Pipeline.MaxProgressPublisher = actionPublisher;

    public void SetMessageListener(IMessageListener listener) => _listener = listener;

    public void SetNotificationPublisher(Action<string> publisher) =>
        _chain.Pipeline.NotificationsPublisher = publisher;
}
