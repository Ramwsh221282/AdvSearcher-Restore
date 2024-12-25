using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal interface IVkParserRequestFactory
{
    internal IHttpRequest CreateVkPostOwnerRequest(VkOptions options, string id);

    internal IHttpRequest CreateVkGroupOwnerIdRequest(
        VkOptions options,
        VkRequestParameters parameters
    );

    internal IHttpRequest CreateWallPostRequest(VkOptions options, VkGroupInfo info);
}
