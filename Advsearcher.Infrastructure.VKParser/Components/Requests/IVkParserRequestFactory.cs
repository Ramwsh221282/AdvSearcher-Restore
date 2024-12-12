using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

public interface IVkParserRequestFactory
{
    internal IVkParserRequest CreateVkPostOwnerRequest(
        RestClient client,
        VkOptions options,
        VkPublisher publisher
    );

    internal IVkParserRequest CreateVkGroupOwnerIdRequest(
        RestClient client,
        VkOptions options,
        VkRequestParameters parameters
    );

    internal IVkParserRequest CreateWallPostRequest(
        RestClient client,
        VkOptions options,
        VkGroupInfo info
    );
}
