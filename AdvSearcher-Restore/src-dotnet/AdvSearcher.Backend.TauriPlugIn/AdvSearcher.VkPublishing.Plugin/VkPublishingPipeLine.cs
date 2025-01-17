using AdvSearcher.VkPublishing.Plugin.Requests;

namespace AdvSearcher.VkPublishing.Plugin;

internal sealed class VkPublishingPipeLine
{
    private readonly List<IVkPublishingRequest> _requests = [];

    public VkPublishingPipeLine AddRequest(IVkPublishingRequest request)
    {
        _requests.Add(request);
        return this;
    }

    public async Task Process()
    {
        foreach (IVkPublishingRequest request in _requests)
            await request.Execute();
    }
}
