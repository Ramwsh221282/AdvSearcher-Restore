using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace AdvSearcher.VK.Parser.Components.Requests;

public sealed class GetVkPostOwnerRequest : IHttpRequest
{
    private readonly RestRequest _request;
    public RestRequest Request => _request;

    public GetVkPostOwnerRequest(VKOptions vkOptions, string id)
    {
        var url =
            $"https://api.vk.com/method/users.get?user_ids={id}&fields=bdate,city,country&access_token={vkOptions.OAuthAccessToken}&v={vkOptions.ApiVersion}";
        _request = new RestRequest(url);
    }
}
