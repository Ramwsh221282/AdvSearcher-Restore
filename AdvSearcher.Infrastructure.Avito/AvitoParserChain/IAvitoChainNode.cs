namespace AdvSearcher.Infrastructure.Avito.AvitoParserChain;

internal interface IAvitoChainNode
{
    IAvitoChainNode? Next { get; }
    AvitoParserPipeLine Pipeline { get; }
    Task ExecuteAsync();
}
