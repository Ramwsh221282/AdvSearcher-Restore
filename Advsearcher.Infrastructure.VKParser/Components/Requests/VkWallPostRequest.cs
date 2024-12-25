using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class VkWallPostRequest(VkGroupInfo info, VkOptions options) : IHttpRequest
{
    public RestRequest Request => _request;

    private readonly RestRequest _request =
        new(
            $"https://api.vk.com/method/wall.get?count=100&owner_id=-{info.GroupId}&offset=1&access_token={options.OAuthAccessToken}&v={options.ApiVesrion}"
        );
}
