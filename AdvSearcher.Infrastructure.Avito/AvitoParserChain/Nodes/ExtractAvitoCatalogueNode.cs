using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonQueries;

namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;

internal sealed class ExtractAvitoCatalogueNode : IAvitoChainNode
{
    private readonly AvitoParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public IAvitoChainNode? Next { get; }
    public AvitoParserPipeLine Pipeline => _pipeLine;

    public ExtractAvitoCatalogueNode(
        AvitoParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        IAvitoChainNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Extracting avito catalogue nodes");
        if (_pipeLine.Url == null)
        {
            _logger.Log("Avito parse url was not provided. Stopping process.");
            return;
        }
        _pipeLine.Provider.InstantiateNewWebDriver();
        _logger.Log("Web driver instance created");
        await new NavigateOnPageCommand(_pipeLine.Url.Value.Value).ExecuteAsync(_pipeLine.Provider);
        _logger.Log("Navigating on avito page");
        await Scroll();
        _logger.Log("Page scrolled");
        string html = await new ExtractHtmlQuery().ExecuteAsync(_pipeLine.Provider);
        _logger.Log("Extracted html from page");
        if (string.IsNullOrEmpty(html))
        {
            _logger.Log("Html page was empty. Stopping process.");
            return;
        }
        _pipeLine.SetCatalogueItems(new AvitoCatalogue(html));
        _pipeLine.ProcessFilteringByDate();
        if (Next != null)
        {
            _logger.Log("Next step in chain started.");
            await Next.ExecuteAsync();
        }
    }

    private async Task Scroll()
    {
        bool isScrolled = false;
        while (!isScrolled)
        {
            try
            {
                await new ScrollToBottomCommand().ExecuteAsync(_pipeLine.Provider);
                isScrolled = true;
            }
            catch
            {
                // ignored
            }
        }
    }
}
