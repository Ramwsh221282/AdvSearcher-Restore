using AdvSearcher.Infrastructure.Domclick.HttpRequests;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickDependencyDisposeNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly DomclickRequestHandler _handler;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickDependencyDisposeNode(
        DomclickParserPipeline pipeline,
        DomclickRequestHandler handler,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        _handler = handler;
        Next = next;
    }

    public async Task Process()
    {
        _pipeline.Provider.Dispose();
        _handler.Dispose();
        if (Next != null)
            await Next.Process();
    }
}
