using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Models.HttpSenders;

internal sealed class VkHttpSender : IVkHttpSender
{
    private const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";

    public RestClient ProvideRestClient()
    {
        var options = new RestClientOptions() { UserAgent = UserAgent };
        var client = new RestClient(options);
        return client;
    }
}
