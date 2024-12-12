using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class VkWallPostRequest(RestClient client, VkGroupInfo info, VkOptions options)
    : IVkParserRequest
{
    private readonly RestRequest _request =
        new(
            $"https://api.vk.com/method/wall.get?count=100&owner_id=-{info.GroupId}&offset=1&access_token={options.OAuthAccessToken}&v={options.ApiVesrion}"
        );

    public async Task<string?> ExecuteAsync()
    {
        var response = await client.ExecuteAsync(_request);
        return response.Content;
    }
}
