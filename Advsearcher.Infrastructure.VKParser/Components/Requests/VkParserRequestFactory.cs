using AdvSearcher.Parser.SDK.HttpParsing;

namespace Advsearcher.Infrastructure.VKParser.Components.Requests;

internal sealed class VkParserRequestFactory : IVkParserRequestFactory
{
    public IHttpRequest CreateVkPostOwnerRequest(VkOptions options, string id) =>
        new GetVkPostOwnerRequest(options, id);

    public IHttpRequest CreateVkGroupOwnerIdRequest(
        VkOptions options,
        VkRequestParameters parameters
    ) => new VkGroupOwnerIdRequest(options, parameters);

    public IHttpRequest CreateWallPostRequest(VkOptions options, VkGroupInfo info) =>
        new VkWallPostRequest(info, options);
}
