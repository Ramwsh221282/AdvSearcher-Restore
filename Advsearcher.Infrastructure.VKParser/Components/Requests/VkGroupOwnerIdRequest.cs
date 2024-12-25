using AdvSearcher.Parser.SDK.HttpParsing;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class VkGroupOwnerIdRequest(VkOptions options, VkRequestParameters parameters)
    : IHttpRequest
{
    public RestRequest Request => _request;

    private readonly RestRequest _request =
        new(
            $@"https://api.vk.com/method/utils.resolveScreenName?screen_name={parameters.ScreenName}&access_token={options.ServiceAccessToken}&v={options.ApiVesrion}"
        );
}
