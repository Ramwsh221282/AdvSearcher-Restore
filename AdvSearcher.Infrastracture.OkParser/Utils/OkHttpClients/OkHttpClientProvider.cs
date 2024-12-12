using RestSharp;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkHttpClients;

internal sealed class OkHttpClientProvider : IOkHttpClientProvider
{
    private const string UserAgentValue =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";

    public RestClient Provide()
    {
        var options = new RestClientOptions() { UserAgent = UserAgentValue };
        var client = new RestClient(options);
        return client;
    }
}
