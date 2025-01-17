using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace AdvSearcher.VK.Parser.Components.Requests;

public sealed class VkWallPostRequest(VkGroupInfo info, VKOptions options) : IHttpRequest
{
    public RestRequest Request => _request;

    private readonly RestRequest _request =
        new(
            $"https://api.vk.com/method/wall.get?count=100&owner_id=-{info.GroupId}&access_token={options.OAuthAccessToken}&v={options.ApiVersion}"
        );
}
