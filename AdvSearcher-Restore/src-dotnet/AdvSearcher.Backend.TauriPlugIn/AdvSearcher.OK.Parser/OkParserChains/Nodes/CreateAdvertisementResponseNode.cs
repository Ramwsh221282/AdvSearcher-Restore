using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Tools;
using AdvSearcher.OK.Parser.Filters;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.OK.Parser.OkParserChains.Nodes;

public sealed class CreateAdvertisementResponseNode : IOkParserChain
{
    private readonly OkParserPipeLine _pipeLine;
    private readonly IHttpClient _httpClient;
    private readonly IMessageListener _listener;
    private int _currentProgress;
    public OkParserPipeLine PipeLine => _pipeLine;
    public IOkParserChain? Next { get; }

    public CreateAdvertisementResponseNode(
        OkParserPipeLine pipeLine,
        IHttpClient client,
        IMessageListener listener,
        IOkParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = client;
        _listener = listener;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Создание результатов парсинга ОК.");
        _pipeLine.NotificationsPublisher?.Invoke("Создание результатов парсинга ОК.");
        if (_pipeLine.Nodes == null)
        {
            string message = "Топики ОК не найдены. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        ParserFilter filter = new ParserFilter(_pipeLine.Options);
        _pipeLine.MaxProgressPublisher?.Invoke(_pipeLine.Nodes.Nodes.Count);
        foreach (var node in _pipeLine.Nodes.Nodes)
        {
            _pipeLine.InstantiateUrlBuilder(node);
            _pipeLine.InstantiateDateBuilder(node);
            if (!await IsMatchingDate(filter))
            {
                _currentProgress++;
                _pipeLine.CurrentProgressPublisher?.Invoke(_currentProgress);
                continue;
            }
            _pipeLine.InstantiateContentBuilder(node, _httpClient);
            _pipeLine.InstantiatePublisherBuilder(node);
            await _pipeLine.AddAdvertisementResponse(filter);
            _currentProgress++;
            _pipeLine.CurrentProgressPublisher?.Invoke(_currentProgress);
        }
        _httpClient.Dispose();
        _pipeLine.NotificationsPublisher?.Invoke(
            $"Получено {_pipeLine.Responses.Count} результатов парсинга ОК."
        );
        _listener.Publish($"Получено {_pipeLine.Responses.Count} результатов парсинга ОК.");
        if (Next != null)
            await Next.ExecuteAsync();
    }

    private async Task<bool> IsMatchingDate(ParserFilter filter)
    {
        Result<DateOnly> date = await _pipeLine.GetAdvertisementDate();
        if (date.IsFailure)
            return false;
        IParserFilterVisitor visitor = new OkDateOnlyFilterVisitor(date.Value);
        return filter.IsMatchingFilters(visitor);
    }
}
