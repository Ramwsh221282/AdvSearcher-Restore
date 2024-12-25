namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal interface IDomclickRequestExecutor : IDisposable
{
    Task<string?> ExecuteAsync(IDomclickRequest request);
}
