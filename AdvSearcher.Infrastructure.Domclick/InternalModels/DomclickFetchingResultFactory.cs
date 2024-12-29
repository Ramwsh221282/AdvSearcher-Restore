using Newtonsoft.Json.Linq;

namespace AdvSearcher.Infrastructure.Domclick.InternalModels;

internal sealed class DomclickFetchingResultFactory : IDomclickFetchingResultFactory
{
    public List<DomclickFetchResult> Create(string response)
    {
        List<DomclickFetchResult> results = [];
        try
        {
            JObject jsonObject = JObject.Parse(response);
            JToken? result = jsonObject["result"];
            if (result == null)
                return results;
            JToken? itemsJson = result["items"];
            if (itemsJson == null)
                return results;
            JToken[] itemsArray = itemsJson.ToArray();
            foreach (var item in itemsArray)
            {
                DomclickFetchResult fetchResult = new DomclickFetchResult(item);
                if (!fetchResult.IsAgent)
                    results.Add(fetchResult);
            }
        }
        catch
        {
            // ignored
        }

        return results;
    }
}
