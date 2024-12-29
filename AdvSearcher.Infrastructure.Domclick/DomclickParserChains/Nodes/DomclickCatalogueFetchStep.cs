using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickCatalogueFetchStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly IDomclickFetchingResultFactory _factory;
    private readonly WebDriverProvider _provider;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickCatalogueFetchStep(
        DomclickParserPipeline pipeLine,
        IDomclickFetchingResultFactory factory,
        WebDriverProvider provider,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeLine;
        _factory = factory;
        _provider = provider;
        Next = next;
    }

    public async Task Process()
    {
        DomclickPageRequestSender sender = new DomclickPageRequestSender(
            _factory,
            _pipeline,
            _provider
        );
        await sender.ConstructFetchResults();
        if (Next != null)
            await Next.Process();
    }
}
