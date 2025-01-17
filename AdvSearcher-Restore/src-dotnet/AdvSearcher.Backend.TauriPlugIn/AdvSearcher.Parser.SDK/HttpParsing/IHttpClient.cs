using RestSharp;

namespace AdvSearcher.Parser.SDK.HttpParsing;

public interface IHttpClient
{
    public RestClient Instance { get; }
    public void Dispose();
}
