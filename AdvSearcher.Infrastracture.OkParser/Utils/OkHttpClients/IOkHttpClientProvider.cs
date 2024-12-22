using RestSharp;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkHttpClients;

internal interface IOkHttpClientProvider
{
    RestClient Provide();
}
