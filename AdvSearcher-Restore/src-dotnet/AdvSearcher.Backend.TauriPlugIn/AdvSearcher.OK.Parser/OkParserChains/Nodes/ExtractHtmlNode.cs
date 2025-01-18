using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonQueries;

namespace AdvSearcher.OK.Parser.OkParserChains.Nodes;

public sealed class ExtractHtmlNode : IOkParserChain
{
    private readonly OkParserPipeLine _pipeLine;
    private readonly WebDriverProvider _provider;
    private readonly IMessageListener _listener;
    public OkParserPipeLine PipeLine => _pipeLine;
    public IOkParserChain? Next { get; }

    public ExtractHtmlNode(
        OkParserPipeLine pipeLine,
        WebDriverProvider provider,
        IMessageListener listener,
        IOkParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _provider = provider;
        _listener = listener;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Получение топиков ОК.");
        _pipeLine.NotificationsPublisher?.Invoke("Получение топиков ОК.");
        if (_pipeLine.Url == null)
        {
            string message = "Ссылка на группу ОК некорректна. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        _provider.InstantiateNewWebDriver(1);
        await new NavigateOnPageCommand(_pipeLine.Url.Value.Value).ExecuteAsync(_provider);
        await new ScrollToBottomCommand().ExecuteAsync(_provider);
        string html = await new ExtractHtmlQuery().ExecuteAsync(_provider);
        if (string.IsNullOrWhiteSpace(html))
        {
            string message = "HTML веб-страницы не получено. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        _pipeLine.SetNodes(new OkPageHtml(html));
        _provider.Dispose();
        _pipeLine.NotificationsPublisher?.Invoke("Топики ОК проинициализированы.");
        _listener.Publish("Топики ОК проинициализированы.");
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
