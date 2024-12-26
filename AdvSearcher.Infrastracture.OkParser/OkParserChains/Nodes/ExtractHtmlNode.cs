using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonQueries;

namespace AdvSearcher.Infrastracture.OkParser.OkParserChains.Nodes;

internal sealed class ExtractHtmlNode : IOkParserChain
{
    private readonly OkParserPipeLine _pipeLine;
    private readonly WebDriverProvider _provider;
    public OkParserPipeLine PipeLine => _pipeLine;
    public IOkParserChain? Next { get; }

    public ExtractHtmlNode(
        OkParserPipeLine pipeLine,
        WebDriverProvider provider,
        IOkParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _provider = provider;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.Url == null)
            return;

        _provider.InstantiateNewWebDriver();
        await new NavigateOnPageCommand(_pipeLine.Url.Url.Value).ExecuteAsync(_provider);
        await new ScrollToBottomCommand().ExecuteAsync(_provider);
        string html = await new ExtractHtmlQuery().ExecuteAsync(_provider);
        _provider.Dispose();
        if (string.IsNullOrWhiteSpace(html))
            return;
        _pipeLine.SetNodes(new OkPageHtml(html));
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
