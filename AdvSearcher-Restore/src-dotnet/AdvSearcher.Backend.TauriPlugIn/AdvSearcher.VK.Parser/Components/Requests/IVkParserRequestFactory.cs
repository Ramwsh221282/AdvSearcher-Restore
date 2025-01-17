using AdvSearcher.Parser.SDK.HttpParsing;

namespace AdvSearcher.VK.Parser.Components.Requests;

public interface IVkParserRequestFactory
{
    internal IHttpRequest CreateVkPostOwnerRequest(VKOptions options, string id);

    internal IHttpRequest CreateVkGroupOwnerIdRequest(
        VKOptions options,
        VkRequestParameters parameters
    );

    internal IHttpRequest CreateWallPostRequest(VKOptions options, VkGroupInfo info);
}
