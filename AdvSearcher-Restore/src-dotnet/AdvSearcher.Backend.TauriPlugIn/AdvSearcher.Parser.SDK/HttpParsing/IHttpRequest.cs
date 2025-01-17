using RestSharp;

namespace AdvSearcher.Parser.SDK.HttpParsing;

public interface IHttpRequest
{
    public RestRequest Request { get; }
}
