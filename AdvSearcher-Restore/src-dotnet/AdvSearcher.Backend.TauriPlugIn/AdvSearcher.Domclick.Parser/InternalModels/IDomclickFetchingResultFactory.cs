namespace AdvSearcher.Domclick.Parser.InternalModels;

public interface IDomclickFetchingResultFactory
{
    List<DomclickFetchResult> Create(string response);
}
