using AdvSearcher.Core.Tools;

namespace AdvSearcher.VkPublishing.Plugin.Requests;

internal sealed class ResponsesContainer
{
    private readonly List<VkPublishingResponse> _responses = [];

    public void AddResponse(VkPublishingResponse response) => _responses.Add(response);

    public Result<T> GetResponseOfType<T>()
        where T : VkPublishingResponse
    {
        string type = typeof(T).Name;
        VkPublishingResponse? response = _responses.FirstOrDefault(r => r.GetType().Name == type);
        if (response is null)
            return new Error($"Response of type {type} was not executed");
        return (T)response;
    }

    public IEnumerable<T> GetResponsesOfType<T>()
        where T : VkPublishingResponse
    {
        string type = typeof(T).Name;
        IEnumerable<VkPublishingResponse> responses = _responses
            .Select(r => r)
            .Where(r => r.GetType().Name == type);
        List<T> downCasted = [];
        foreach (var item in responses)
            downCasted.Add((T)item);
        return downCasted;
    }

    public void CleanFromResponsesOfType<T>()
        where T : VkPublishingResponse
    {
        string type = typeof(T).Name;
        _responses.RemoveAll(r => r.GetType().Name == type);
    }
}
