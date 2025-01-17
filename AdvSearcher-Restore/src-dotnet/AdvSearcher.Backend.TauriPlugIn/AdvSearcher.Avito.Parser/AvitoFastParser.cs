using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Avito.Parser.Steps;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;

namespace AdvSearcher.Avito.Parser;

public sealed class AvitoFastParser : IParser
{
    private readonly IAvitoFastParserStep _step;
    private readonly List<IParserResponse> _results = [];
    private IMessageListener? _listener;
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public AvitoFastParser(IAvitoFastParserStep step) => _step = step;

    public async Task<Result<bool>> ParseData(
        ServiceUrl url,
        List<ParserFilterOption>? options = null
    )
    {
        if (options != null)
        {
            _listener?.Publish("Фильтры применены");
            _step.Pipeline.Options = options;
        }
        _step.Pipeline.SetServiceUrl(url);
        await _step.ProcessAsync();
        _results.AddRange(_step.Pipeline.Results);
        return true;
    }

    public void SetCurrentProgressValuePublisher(Action<int> actionPublisher) =>
        _step.Pipeline.CurrentProgressPublisher = actionPublisher;

    public void SetMaxProgressValuePublisher(Action<int> actionPublisher) =>
        _step.Pipeline.MaxProgressPublisher = actionPublisher;

    public void SetMessageListener(IMessageListener listener) => _listener = listener;

    public void SetNotificationPublisher(Action<string> publisher) =>
        _step.Pipeline.NotificationsPublisher = publisher;
}
