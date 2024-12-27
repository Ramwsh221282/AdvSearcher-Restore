namespace AdvSearcher.Infrastructure.Domclick.DomclickParserChains;

internal interface IDomclickParserChain
{
    IDomclickParserChain? Next { get; }
    DomclickParserPipeline Pipeline { get; }
    Task Process();
}
