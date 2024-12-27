using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomClickPageHttpFetchNode : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly DomclickRequestHandler _handler;
    private readonly IDomclickFetchingResultFactory _factory;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomClickPageHttpFetchNode(
        DomclickParserPipeline pipeline,
        DomclickRequestHandler handler,
        IDomclickFetchingResultFactory factory,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeline;
        _handler = handler;
        _factory = factory;
        Next = next;
    }

    public async Task Process()
    {
        if (string.IsNullOrWhiteSpace(_pipeline.QratorValue))
            return;
        DomclickPageRequestSender sender = new DomclickPageRequestSender(
            _factory,
            _handler,
            _pipeline.QratorValue
        );
        await sender.ConstructFetchResults();
        if (sender.Results.Count == 0)
            return;
        _pipeline.SetFetchingResults(sender.Results.Where(item => !item.IsAgent).ToArray());
        if (Next != null)
            await Next.Process();
    }
}
