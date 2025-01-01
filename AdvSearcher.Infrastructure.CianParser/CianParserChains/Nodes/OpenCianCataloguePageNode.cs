using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToTop;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class OpenCianCataloguePageNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public OpenCianCataloguePageNode(
        CianParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Opening cian catalogue page");
        if (_pipeLine.Url == null)
        {
            _logger.Log("No catalogue url provided. Stopping process.");
            return;
        }
        _pipeLine.Provider.InstantiateNewWebDriver();
        _logger.Log("Web driver instance created");
        await new NavigateOnPageCommand(_pipeLine.Url.Value.Value).ExecuteAsync(_pipeLine.Provider);
        _logger.Log("Navigated on cian page");
        await new ScrollToBottomCommand().ExecuteAsync(_pipeLine.Provider);
        _logger.Log("Scrolled to bottom");
        await new ScrollToTopCommand().ExecuteAsync(_pipeLine.Provider);
        _logger.Log("Scrolled to top");
        if (Next != null)
        {
            _logger.Log("Executing next process");
            await Next.ExecuteAsync();
        }
    }
}
