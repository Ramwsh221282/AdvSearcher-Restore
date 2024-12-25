using RestSharp;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickHttpSender : IDomclickRequestExecutor
{
    private readonly RestClient _client;

    private const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";

    public DomclickHttpSender()
    {
        var options = new RestClientOptions() { UserAgent = UserAgent };
        _client = new RestClient(options);
    }

    public void Dispose() => _client.Dispose();

    public async Task<string?> ExecuteAsync(IDomclickRequest request)
    {
        var response = await _client.ExecuteAsync(request.Request);
        return response.Content;
    }
}
