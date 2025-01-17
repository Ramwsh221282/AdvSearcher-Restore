using AdvSearcher.Application.Contracts.MessageListeners;

namespace AdvSearcher.Cian.Parser.CianParserChains.Nodes;

public sealed class ConstructResponseNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    private readonly IMessageListener _listener;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public ConstructResponseNode(
        CianParserPipeLine pipeLine,
        IMessageListener listener,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _listener = listener;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Создание результатов парсинга Циан.");
        _pipeLine.NotificationsPublisher?.Invoke("Создание результатов парсинга Циан.");
        _pipeLine.InstantiateFactory();
        _pipeLine.ConstructResponses();
        _pipeLine.ProcessFiltering();
        _listener.Publish($"Создано {_pipeLine.Responses.Count} результатов");
        _pipeLine.NotificationsPublisher?.Invoke(
            $"Создано {_pipeLine.Responses.Count} результатов"
        );
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
