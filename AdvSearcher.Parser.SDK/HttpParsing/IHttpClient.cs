using RestSharp;

namespace AdvSearcher.Parser.SDK.HttpParsing;

public interface IHttpClient : IDisposable
{
    public RestClient Instance { get; }
}
