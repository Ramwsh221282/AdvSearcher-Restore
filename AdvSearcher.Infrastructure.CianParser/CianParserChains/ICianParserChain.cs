namespace AdvSearcher.Infrastructure.CianParser.CianParserChains;

internal interface ICianParserChain
{
    CianParserPipeLine PipeLine { get; }
    ICianParserChain? Next { get; }
    Task ExecuteAsync();
}
