namespace AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;

internal sealed class ConstructResponseNode : ICianParserChain
{
    private readonly CianParserPipeLine _pipeLine;
    public CianParserPipeLine PipeLine => _pipeLine;
    public ICianParserChain? Next { get; }

    public ConstructResponseNode(CianParserPipeLine pipeLine, ICianParserChain? next = null)
    {
        _pipeLine = pipeLine;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _pipeLine.InstantiateFactory();
        _pipeLine.ConstructResponses();
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
