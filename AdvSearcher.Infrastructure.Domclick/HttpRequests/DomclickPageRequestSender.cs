using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.HttpParsing;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Infrastructure.Domclick.HttpRequests;

internal sealed class DomclickPageRequestSender
{
    private readonly IDomclickFetchingResultFactory _factory;
    private readonly DomclickRequestHandler _handler;
    private readonly string _qratorValue;
    private int _offset = 20;
    private int _maxCount;
    private readonly List<DomclickFetchResult> _results = [];

    public IReadOnlyCollection<DomclickFetchResult> Results => _results;

    public DomclickPageRequestSender(
        IDomclickFetchingResultFactory factory,
        DomclickRequestHandler handler,
        string qratorValue
    )
    {
        _factory = factory;
        _handler = handler;
        _qratorValue = qratorValue;
    }

    public async Task ConstructFetchResults()
    {
        bool IsNotFirstFetch = true;
        while (true)
        {
            if (this._offset + 20 < this._maxCount || IsNotFirstFetch)
            {
                IHttpRequest request = new DomclickGetPageRequest(_qratorValue, _offset);
                string response = await _handler.ForceExecuteAsync(request);
                UpdateMaxCount(response);
                UpdateOffset();
                ConstructFetchResultsFromResponse(response);
                IsNotFirstFetch = false;
            }
            else
                break;
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
