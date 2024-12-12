using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Models.HttpSenders;

public interface IVkHttpSender
{
    public RestClient ProvideRestClient();
}
