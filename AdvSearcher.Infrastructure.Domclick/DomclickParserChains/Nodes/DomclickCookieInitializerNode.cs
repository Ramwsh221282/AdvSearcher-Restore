using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickCookieInitializerNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickCookieInitializerNode(
        DomclickParserPipeline pipeline,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        Next = next;
    }

    public async Task Process()
    {
        if (_pipeline.DomclickItems == null)
            return;
        if (_pipeline.DomclickItems.Length == 0)
            return;
        DomclickFetchResult firstItem = _pipeline.DomclickItems[0];
        if (string.IsNullOrWhiteSpace(firstItem.SourceUrl))
            return;
        await new NavigateOnPageCommand(firstItem.SourceUrl).ExecuteAsync(_pipeline.Provider);
        _pipeline.InstantiateCookieHeaderValue();
        if (Next != null)
            await Next.Process();
    }
}
