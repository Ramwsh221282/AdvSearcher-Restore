namespace AdvSearcher.Cian.Parser.CianParserChains;

public interface ICianParserChain
{
    CianParserPipeLine PipeLine { get; }
    ICianParserChain? Next { get; }
    Task ExecuteAsync();
}
