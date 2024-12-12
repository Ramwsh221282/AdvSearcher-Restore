using RestSharp;

namespace AdvSearcher.Infrastracture.OkParser.Utils.OkHttpClients;

public interface IOkHttpClientProvider
{
    RestClient Provide();
}
