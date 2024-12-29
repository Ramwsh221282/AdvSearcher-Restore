using RestSharp;

namespace AdvSearcher.Parser.SDK.HttpParsing;

internal sealed class RestHttpClient : IHttpClient
{
    private readonly RestClient _instance;
    public RestClient Instance => _instance;

    private const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";

    public RestHttpClient()
    {
        var options = new RestClientOptions() { UserAgent = UserAgent };
        _instance = new RestClient();
    }

    public void Dispose()
    {
        _instance.Dispose();
        Instance.Dispose();
    }
}
