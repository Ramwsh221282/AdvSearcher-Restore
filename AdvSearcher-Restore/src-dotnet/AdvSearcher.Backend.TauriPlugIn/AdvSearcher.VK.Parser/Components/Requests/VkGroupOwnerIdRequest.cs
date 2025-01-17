using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace AdvSearcher.VK.Parser.Components.Requests;

public sealed class VkGroupOwnerIdRequest(VKOptions options, VkRequestParameters parameters)
    : IHttpRequest
{
    public RestRequest Request => _request;

    private readonly RestRequest _request =
        new(
            $@"https://api.vk.com/method/utils.resolveScreenName?screen_name={parameters.ScreenName}&access_token={options.ServiceAccessToken}&v={options.ApiVersion}"
        );
}
