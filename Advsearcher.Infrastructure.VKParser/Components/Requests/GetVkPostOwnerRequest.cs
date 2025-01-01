using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class GetVkPostOwnerRequest : IHttpRequest
{
    private readonly RestRequest _request;
    public RestRequest Request => _request;

    public GetVkPostOwnerRequest(VkOptions vkOptions, string id)
    {
        var url =
            $"https://api.vk.com/method/users.get?user_ids={id}&fields=bdate,city,country&access_token={vkOptions.OAuthAccessToken}&v={vkOptions.ApiVesrion}";
        _request = new RestRequest(url);
    }
}
