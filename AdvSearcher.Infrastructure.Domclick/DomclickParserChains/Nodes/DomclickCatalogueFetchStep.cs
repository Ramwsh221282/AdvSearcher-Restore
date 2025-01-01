using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.WebDriverParsing;

namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;

internal sealed class DomclickCatalogueFetchStep : IDomclickParserChain
{
    private readonly DomclickParserPipeline _pipeline;
    private readonly IDomclickFetchingResultFactory _factory;
    private readonly WebDriverProvider _provider;
    private readonly ParserConsoleLogger _logger;
    public IDomclickParserChain? Next { get; }
    public DomclickParserPipeline Pipeline => _pipeline;

    public DomclickCatalogueFetchStep(
        DomclickParserPipeline pipeLine,
        IDomclickFetchingResultFactory factory,
        WebDriverProvider provider,
        ParserConsoleLogger logger,
        IDomclickParserChain? next = null
    )
    {
        _pipeline = pipeLine;
        _factory = factory;
        _provider = provider;
        _logger = logger;
        Next = next;
    }

    public async Task Process()
    {
        _logger.Log("Creating domclick response results.");
        DomclickPageRequestSender sender = new DomclickPageRequestSender(
            _factory,
            _pipeline,
            _provider
        );
        await sender.ConstructFetchResults();
        _pipeline.FilterByCacheAndDate();
        _logger.Log($"Domclick response results created. Count: {_pipeline.Responses.Count}");
        if (Next != null)
        {
            _logger.Log("Processing next step.");
            await Next.Process();
        }
    }
}
