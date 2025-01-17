using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.VK.Parser.Components.Requests;

public sealed class VkParserRequestFactory : IVkParserRequestFactory
{
    public IHttpRequest CreateVkPostOwnerRequest(VKOptions options, string id) =>
        new GetVkPostOwnerRequest(options, id);

    public IHttpRequest CreateVkGroupOwnerIdRequest(
        VKOptions options,
        VkRequestParameters parameters
    ) => new VkGroupOwnerIdRequest(options, parameters);

    public IHttpRequest CreateWallPostRequest(VKOptions options, VkGroupInfo info) =>
        new VkWallPostRequest(info, options);
}
