namespace AdvSearcher.OK.Parser.OkParserChains;

public interface IOkParserChain
{
    OkParserPipeLine PipeLine { get; }
    IOkParserChain? Next { get; }
    Task ExecuteAsync();
}
