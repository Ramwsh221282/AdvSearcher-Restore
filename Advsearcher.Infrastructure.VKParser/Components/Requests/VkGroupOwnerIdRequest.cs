using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class VkGroupOwnerIdRequest(
    RestClient client,
    VkOptions options,
    VkRequestParameters parameters
) : IVkParserRequest
{
    private readonly RestRequest _request =
        new(
            $@"https://api.vk.com/method/utils.resolveScreenName?screen_name={parameters.ScreenName}&access_token={options.ServiceAccessToken}&v={options.ApiVesrion}"
        );

    public async Task<string?> ExecuteAsync()
    {
        var response = await client.ExecuteAsync(_request);
        return response.Content;
    }
}
