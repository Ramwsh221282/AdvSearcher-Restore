using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Domclick.Parser.HttpRequests;
using AdvSearcher.Domclick.Parser.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Domclick.Parser.DomclickParserChains.Nodes;

internal sealed class DomclickCatalogueFetchStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly IDomclickFetchingResultFactory _factory;
    private readonly WebDriverProvider _provider;
    private readonly IMessageListener _listener;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickCatalogueFetchStep(
        DomclickParserPipeline pipeLine,
        IDomclickFetchingResultFactory factory,
        WebDriverProvider provider,
        IMessageListener listener,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeLine;
        _factory = factory;
        _provider = provider;
        _listener = listener;
        Next = next;
    }

    public async Task Process()
    {
        _listener.Publish("Получение данных каталога ДомКлик.");
        _pipeline.NotificationsPublisher?.Invoke("Получение данных каталога ДомКлик.");
        DomclickPageRequestSender sender = new DomclickPageRequestSender(
            _factory,
            _pipeline,
            _provider
        );
        await sender.ConstructFetchResults();
        _pipeline.NotificationsPublisher?.Invoke("Данные каталога ДомКлик получены.");
        _listener.Publish("Данные каталога ДомКлик получены.");
        if (Next != null)
            await Next.Process();
    }
}
