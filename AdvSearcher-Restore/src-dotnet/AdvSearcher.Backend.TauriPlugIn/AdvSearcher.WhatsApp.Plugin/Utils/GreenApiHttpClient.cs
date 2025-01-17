using RestSharp;

namespace AdvSearcher.WhatsApp.Plugin.Utils;

internal sealed class GreenApiHttpClient
{
    private readonly RestClient _client;
    private bool _isDestroyed;

    public GreenApiHttpClient()
    {
        _client = new RestClient();
    }

    public void Destroy()
    {
        _client.Dispose();
        _isDestroyed = true;
    }

    public bool IsAvailable => !_isDestroyed;

    public RestClient Client => _client;
}
