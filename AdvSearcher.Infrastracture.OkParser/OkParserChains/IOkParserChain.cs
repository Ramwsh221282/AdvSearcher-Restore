namespace AdvSearcher.Infrastracture.OkParser.OkParserChains;

internal interface IOkParserChain
{
    OkParserPipeLine PipeLine { get; }
    IOkParserChain? Next { get; }
    Task ExecuteAsync();
}
