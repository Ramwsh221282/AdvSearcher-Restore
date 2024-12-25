namespace AdvSearcher.Infrastructure.Domclick.InternalModels;

internal interface IDomclickFetchingResultFactory
{
    List<DomclickFetchResult> Create(string response);
}
