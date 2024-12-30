using AdvSearcher.Parser.SDK;

namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class ConstructResponseNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public ConstructResponseNode(
        CianParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        ICianParserChain? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Constructing Cian Advertisements Response");
        _pipeLine.InstantiateFactory();
        _pipeLine.ConstructResponses();
        _logger.Log($"Cian Response constructed. Count: {_pipeLine.Responses.Count}");
        if (Next != null)
        {
            _logger.Log("Processing next step in chain");
            await Next.ExecuteAsync();
        }
    }
}
