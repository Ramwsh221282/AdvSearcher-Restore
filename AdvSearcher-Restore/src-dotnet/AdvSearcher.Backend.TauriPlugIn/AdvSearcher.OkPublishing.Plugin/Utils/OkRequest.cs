using RestSharp;

namespace AdvSearcher.OkPublishing.Plugin.Utils;

internal sealed class OkRequest
{
    private readonly RestRequest _request;

    public OkRequest(RestRequest request) => _request = request;

    public RestRequest Request => _request;
}
