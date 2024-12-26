namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;

internal sealed class ConstructAvitoResponseNode : IAvitoChainNode
{
    private readonly AvitoParserPipeLine _pipeLine;
    public IAvitoChainNode? Next { get; }
    public AvitoParserPipeLine Pipeline => _pipeLine;

    public ConstructAvitoResponseNode(AvitoParserPipeLine pipeLine, IAvitoChainNode? next = null)
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _pipeLine.ConstructParserResponse();
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
