using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser;

internal sealed class VkParsingMethod(IVkParserRequestFactory factory)
{
    public async Task<Result<VkGroupInfo>> GetGroupInfoAsync(
        RestClient client,
        VkOptions options,
        ServiceUrl url
    )
    {
        var parameters = new VkRequestParameters(url);
        var request = factory.CreateVkGroupOwnerIdRequest(client, options, parameters);
        var response = await request.ExecuteAsync();
        if (string.IsNullOrWhiteSpace(response))
            return new Error("Не удалось получить информацию о группе");
        return new VkGroupInfo(new VkGroupResponse(response), url);
    }

    public async Task<Result<VkItemsJson>> GetVkWallItemsJsonAsync(
        RestClient client,
        VkOptions options,
        VkGroupInfo info
    )
    {
        var request = factory.CreateWallPostRequest(client, options, info);
        var response = await request.ExecuteAsync();
        if (string.IsNullOrWhiteSpace(response))
            return new Error("Не удалось получить посты группы");
        return new VkItemsJson(response);
    }
}
