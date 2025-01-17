using AdvSearcher.Core.Tools;

namespace AdvSearcher.OkPublishing.Plugin.Responses;

internal sealed class ResponseContainer
{
    private readonly List<OkResponse> _responses = [];

    public void AddResponse(OkResponse response) => _responses.Add(response);

    public void CleanContainer() => _responses.Clear();

    public Result<T> GetRequiredResponseOfType<T>()
        where T : OkResponse
    {
        string type = typeof(T).Name;
        OkResponse? response = _responses.FirstOrDefault(r => r.GetType().Name == type);
        if (response is null)
            return new Error("No response found of requested type");
        return (T)response;
    }

    public List<T> GetRequiredResponsesOfType<T>()
        where T : OkResponse
    {
        string type = typeof(T).Name;
        IEnumerable<OkResponse> responses = _responses
            .Select(r => r)
            .Where(r => r.GetType().Name == type);
        List<T> downCasted = [];
        foreach (OkResponse response in responses)
            downCasted.Add((T)response);
        return downCasted;
    }
}
