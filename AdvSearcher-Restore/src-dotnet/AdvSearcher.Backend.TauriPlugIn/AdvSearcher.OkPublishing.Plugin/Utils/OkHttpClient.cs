using RestSharp;

namespace AdvSearcher.OkPublishing.Plugin.Utils;

internal sealed class OkHttpClient
{
    private bool _isDestroyed;
    private RestClient _client;

    public OkHttpClient()
    {
        _client = new RestClient(
            new RestClientOptions() { BaseUrl = new Uri("https://api.ok.ru/") }
        );
    }

    public void Destroy()
    {
        _client.Dispose();
        _isDestroyed = true;
    }

    public bool IsAvailable => !_isDestroyed;
    public RestClient Client => _client;
}
