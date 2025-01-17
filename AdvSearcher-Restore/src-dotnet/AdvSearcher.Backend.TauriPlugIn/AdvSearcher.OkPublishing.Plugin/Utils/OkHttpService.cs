using AdvSearcher.Core.Tools;
using RestSharp;

namespace AdvSearcher.OkPublishing.Plugin.Utils;

internal sealed class OkHttpService
{
    private readonly OkHttpClient _client;

    public OkHttpService(OkHttpClient client) => _client = client;

    public async Task<Result<string>> ExecuteAsync(OkRequest request)
    {
        if (!_client.IsAvailable)
            return new Error("Http client is not available");
        RestResponse response = await _client.Client.ExecuteAsync(request.Request);
        string? content = response.Content;
        if (string.IsNullOrWhiteSpace(content))
            return new Error("No response content");
        return content;
    }
}
