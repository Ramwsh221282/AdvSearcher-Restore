namespace AdvSearcher.OkPublishing.Plugin.Requests;

internal sealed class OkRequestsPipeLine
{
    private readonly List<IOkRequest> _requests = [];

    public OkRequestsPipeLine AddRequest(IOkRequest request)
    {
        _requests.Add(request);
        return this;
    }

    public async Task ProcessAll()
    {
        foreach (var request in _requests)
            await request.Execute();
    }
}
