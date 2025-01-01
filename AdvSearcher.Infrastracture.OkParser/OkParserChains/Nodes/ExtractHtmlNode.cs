using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.ScrollToBottom;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonQueries;

namespace AdvSearcher.Infrastracture.OkParser.OkParserChains.Nodes;

internal sealed class ExtractHtmlNode : IOkParserChain
{
    private readonly OkParserPipeLine _pipeLine;
    private readonly WebDriverProvider _provider;
    private readonly ParserConsoleLogger _logger;
    public OkParserPipeLine PipeLine => _pipeLine;
    public IOkParserChain? Next { get; }

    public ExtractHtmlNode(
        OkParserPipeLine pipeLine,
        WebDriverProvider provider,
        ParserConsoleLogger logger,
        IOkParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _provider = provider;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Extracting OK Html nodes");
        if (_pipeLine.Url == null)
        {
            _logger.Log("Pipeline Url was null. Stopping process.");
            return;
        }
        _provider.InstantiateNewWebDriver();
        await new NavigateOnPageCommand(_pipeLine.Url.Value.Value).ExecuteAsync(_provider);
        await new ScrollToBottomCommand().ExecuteAsync(_provider);
        string html = await new ExtractHtmlQuery().ExecuteAsync(_provider);
        _provider.Dispose();
        if (string.IsNullOrWhiteSpace(html))
        {
            _logger.Log("Html was null. Stopping process.");
            return;
        }
        _pipeLine.SetNodes(new OkPageHtml(html));
        _logger.Log("Ok Nodes were initialized");
        if (Next != null)
        {
            _logger.Log("Processing next chain node");
            await Next.ExecuteAsync();
        }
    }
}
