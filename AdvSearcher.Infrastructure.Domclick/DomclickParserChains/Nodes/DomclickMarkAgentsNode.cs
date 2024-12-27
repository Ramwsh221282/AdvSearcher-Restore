using AdvSearcher.Infrastructure.Domclick.DomclickWebDriver.Queries.ExtractPhoneNumber;
using AdvSearcher.Parser.SDK.WebDriverParsing.CommonCommands.NavigateOnPage;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickMarkAgentsNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickMarkAgentsNode(
        DomclickParserPipeline pipeline,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        Next = next;
    }

    public async Task Process()
    {
        if (_pipeline.ResearchApiToken == null)
            return;
        if (_pipeline.DomclickItems == null)
            return;
        foreach (var item in _pipeline.DomclickItems)
        {
            await new NavigateOnPageCommand(item.SourceUrl).ExecuteAsync(_pipeline.Provider);
            bool isNotHomeOwner = await new IsNotHomeownerQuery().ExecuteAsync(_pipeline.Provider);
            if (!isNotHomeOwner)
                item.MarkAsAgent();
        }
        if (Next != null)
            await Next.Process();
    }
}
