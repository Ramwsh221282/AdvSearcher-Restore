using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonQueries;

namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;

internal sealed class ExtractAvitoCatalogueNode : IAvitoChainNode
{
    private readonly AvitoParserPipeLine _pipeLine;
    public IAvitoChainNode? Next { get; }
    public AvitoParserPipeLine Pipeline => _pipeLine;

    public ExtractAvitoCatalogueNode(AvitoParserPipeLine pipeLine, IAvitoChainNode? next = null)
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.Url == null)
            return;

        _pipeLine.Provider.InstantiateNewWebDriver();
        await new NavigateOnPageCommand(_pipeLine.Url.Url.Value).ExecuteAsync(_pipeLine.Provider);
        await Scroll();
        string html = await new ExtractHtmlQuery().ExecuteAsync(_pipeLine.Provider);
        if (string.IsNullOrEmpty(html))
            return;
        _pipeLine.SetCatalogueItems(new AvitoCatalogue(html));
        if (Next != null)
            await Next.ExecuteAsync();
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
