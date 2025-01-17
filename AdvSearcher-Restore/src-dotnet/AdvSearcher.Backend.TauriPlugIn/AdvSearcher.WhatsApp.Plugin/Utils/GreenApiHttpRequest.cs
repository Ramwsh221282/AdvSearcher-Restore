using RestSharp;

namespace AdvSearcher.WhatsApp.Plugin.Utils;

internal sealed class GreenApiHttpRequest
{
    private readonly RestRequest _request;

    public GreenApiHttpRequest(RestRequest request)
    {
        _request = request;
    }

    public RestRequest Request => _request;
}
