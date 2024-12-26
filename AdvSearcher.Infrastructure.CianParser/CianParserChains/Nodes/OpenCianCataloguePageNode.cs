using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToTop;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class OpenCianCataloguePageNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public OpenCianCataloguePageNode(CianParserPipeLine pipeLine, ICianParserChain? next = null)
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
        await new ScrollToBottomCommand().ExecuteAsync(_pipeLine.Provider);
        await new ScrollToTopCommand().ExecuteAsync(_pipeLine.Provider);

        if (Next != null)
            await Next.ExecuteAsync();
    }
}
