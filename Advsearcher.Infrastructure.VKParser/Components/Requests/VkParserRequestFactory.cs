using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class VkParserRequestFactory : IVkParserRequestFactory
{
    public IVkParserRequest CreateVkPostOwnerRequest(
        RestClient client,
        VkOptions options,
        VkPublisher publisher
    ) => new GetVkPostOwnerRequest(client, options, publisher);

    public IVkParserRequest CreateVkGroupOwnerIdRequest(
        RestClient client,
        VkOptions options,
        VkRequestParameters parameters
    ) => new VkGroupOwnerIdRequest(client, options, parameters);

    public IVkParserRequest CreateWallPostRequest(
        RestClient client,
        VkOptions options,
        VkGroupInfo info
    ) => new VkWallPostRequest(client, info, options);
}
