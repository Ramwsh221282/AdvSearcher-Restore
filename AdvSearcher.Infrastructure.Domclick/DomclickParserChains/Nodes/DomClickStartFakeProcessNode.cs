namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomClickStartFakeProcessNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private const string Url = "https://krasnoyarsk.domclick.ru/";
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomClickStartFakeProcessNode(
        DomclickParserPipeline pipeline,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        Next = next;
    }

    public async Task Process()
    {
        _pipeline.Provider.InstantiateNewWebDriver(Url);
        if (Next != null)
            await Next.Process();
    }
}
