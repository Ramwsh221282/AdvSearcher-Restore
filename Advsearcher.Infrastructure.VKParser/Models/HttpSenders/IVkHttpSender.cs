using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Models.HttpSenders;

internal interface IVkHttpSender
{
    public RestClient ProvideRestClient();
}
