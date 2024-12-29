using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickRequestHandler
{
    private readonly IHttpClient _httpClient;
    private readonly IHttpService _httpService;

    public DomclickRequestHandler(IHttpClient httpClient, IHttpService httpService)
    {
        _httpClient = httpClient;
        _httpService = httpService;
    }

    public async Task<string> ExecuteAsync(IHttpRequest request)
    {
        var result = await _httpClient.Instance.ExecuteAsync(request.Request);
        var cookies = result.Cookies;
        var headers = result.Headers;

        var bytes = await _httpClient.Instance.DownloadStreamAsync(request.Request);
        if (bytes != null)
        {
            using (bytes)
            {
                using (StreamReader reader = new StreamReader(bytes))
                {
                    string text = reader.ReadToEnd();
                    int bpoint = 0;
                }
            }
        }

        await _httpService.Execute(_httpClient, request);
        return _httpService.Result;
    }

    public async Task<string> ForceExecuteAsync(IHttpRequest request)
    {
        string response = string.Empty;
        while (string.IsNullOrWhiteSpace(response))
        {
            await _httpService.Execute(_httpClient, request);
            await Task.Delay(TimeSpan.FromSeconds(5));
            response = _httpService.Result;
        }
        return response;
    }

    public void Dispose() => _httpClient.Dispose();
}
