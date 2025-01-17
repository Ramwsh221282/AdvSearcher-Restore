namespace AdvSearcher.Domclick.Parser.DomclickParserChains;

public interface IDomclickParserChain
{
    IDomclickParserChain? Next { get; }
    DomclickParserPipeline Pipeline { get; }
    Task Process();
}
