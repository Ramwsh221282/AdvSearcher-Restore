using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;

namespace AdvSearcher.Cian.Parser.CianParserChains.Nodes;

public sealed class OpenCianCataloguePageNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    private readonly IMessageListener _listener;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public OpenCianCataloguePageNode(
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
        _pipeLine.NotificationsPublisher?.Invoke("Открытие страницы Циан.");
        _listener.Publish("Открытие страницы Циан.");
        if (_pipeLine.Url == null)
        {
            string message = "Ссылка на страницу Циан не предоставлена. Остановка процесса.";
            _listener.Publish(message);
            return;
        }
        _pipeLine.Provider.InstantiateNewWebDriver(1);
        _listener.Publish("Веб драйвер запущен");
        _pipeLine.NotificationsPublisher?.Invoke("Веб драйвер запущен");
        await new NavigateOnPageCommand(_pipeLine.Url.Value.Value).ExecuteAsync(_pipeLine.Provider);
        await new ScrollToBottomCommand().ExecuteAsync(_pipeLine.Provider);
        await new ScrollToTopCommand().ExecuteAsync(_pipeLine.Provider);
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
