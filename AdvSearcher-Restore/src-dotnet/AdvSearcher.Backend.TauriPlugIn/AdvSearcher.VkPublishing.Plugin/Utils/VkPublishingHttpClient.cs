using RestSharp;

namespace AdvSearcher.VkPublishing.Plugin.Utils;

internal sealed class VkPublishingHttpClient
{
    private RestClient? _restClient;

    public void Initialize() => _restClient = new RestClient();

    public void Destroy() => _restClient?.Dispose();

    public RestClient? Client => _restClient;
}
