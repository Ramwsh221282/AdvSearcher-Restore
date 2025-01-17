using AdvSearcher.Core.Tools;
using RestSharp;

namespace AdvSearcher.WhatsApp.Plugin.Utils;

internal sealed class GreenApiHttpService
{
    private readonly GreenApiHttpClient _client;

    public GreenApiHttpService(GreenApiHttpClient client)
    {
        _client = client;
    }

    public async Task<Result<string>> ExecuteAsync(GreenApiHttpRequest request)
    {
        if (!_client.IsAvailable)
            return new Error("Http Client is not available");
        RestResponse response = await _client.Client.ExecuteAsync(request.Request);
        string? content = response.Content;
        return content switch
        {
            null => new Error("No response content"),
            not null when string.IsNullOrWhiteSpace(content) => new Error("No response content"),
            _ => content,
        };
    }
}
