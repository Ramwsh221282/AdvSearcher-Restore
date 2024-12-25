using AdvSearcher.Infrastructure.Domclick.InternalModels;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickPageRequestSender
{
    private readonly IDomclickRequestExecutor _executor;
    private readonly IDomclickFetchingResultFactory _factory;
    private int _offset = 20;
    private int _maxCount;
    private readonly List<DomclickFetchResult> _results = [];

    public IReadOnlyCollection<DomclickFetchResult> Results => _results;

    public DomclickPageRequestSender(
        IDomclickRequestExecutor executor,
        IDomclickFetchingResultFactory factory
    )
    {
        _executor = executor;
        _factory = factory;
    }

    public async Task ConstructFetchResults()
    {
        bool flag = true;
        try
        {
            while (true)
            {
                if (this._offset + 20 < this._maxCount || flag)
                {
                    flag = false;
                    IDomclickRequest request = new DomclickGetPageRequest(_offset);
                    string? response = await _executor.ExecuteAsync(request);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    UpdateMaxCount(response);
                    UpdateOffset();
                    ConstructFetchResultsFromResponse(response);
                }
                else
                    break;
            }

            _executor.Dispose();
        }
        catch
        {
            _executor.Dispose();
        }
    }

    private void UpdateOffset() => _offset += 20;

    private void UpdateMaxCount(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return;
        if (this._maxCount != 0)
            return;
        JObject jsonObject = JObject.Parse(response);
        JToken? result = jsonObject["result"];
        if (result == null)
            return;
        JToken? pagination = result["pagination"];
        if (pagination == null)
            return;
        JToken? total = pagination["total"];
        if (total == null)
            return;
        _maxCount = int.Parse(total.ToString());
    }

    private void ConstructFetchResultsFromResponse(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return;
        _results.AddRange(_factory.Create(response));
    }
}
