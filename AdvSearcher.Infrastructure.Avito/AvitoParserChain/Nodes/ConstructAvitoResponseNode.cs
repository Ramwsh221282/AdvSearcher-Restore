using AdvSearcher.Parser.SDK;

namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;

internal sealed class ConstructAvitoResponseNode : IAvitoChainNode
{
    private readonly AvitoParserPipeLine _pipeLine;
    private readonly ParserConsoleLogger _logger;
    public IAvitoChainNode? Next { get; }
    public AvitoParserPipeLine Pipeline => _pipeLine;

    public ConstructAvitoResponseNode(
        AvitoParserPipeLine pipeLine,
        ParserConsoleLogger logger,
        IAvitoChainNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _logger.Log("Constructing Avito response node");
        _pipeLine.ProcessFiltering();
        _pipeLine.ConstructParserResponse();
        _logger.Log($"Avito advertisements constructed. Count {_pipeLine.Responses.Count}");
        if (Next != null)
        {
            _logger.Log("Processing next step in chain");
            await Next.ExecuteAsync();
        }
    }
}
