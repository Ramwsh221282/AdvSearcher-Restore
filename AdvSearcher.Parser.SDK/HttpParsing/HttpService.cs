using RestSharp;

namespace AdvSearcher.Parser.SDK.HttpParsing;

internal sealed class HttpService : IHttpService
{
    private string? _result;

    public async Task Execute(IHttpClient client, IHttpRequest request)
    {
        RestResponse response = await client.Instance.ExecuteAsync(request.Request);
        _result = response.Content;
    }

    public string Result => string.IsNullOrEmpty(_result) ? string.Empty : _result;
}
