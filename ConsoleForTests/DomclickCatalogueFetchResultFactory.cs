using Newtonsoft.Json.Linq;

namespace ConsoleForTests;

public class DomclickCatalogueFetchResultFactory
{
    public List<DomclickCatalogueFetchResult> Create(string response)
    {
        List<DomclickCatalogueFetchResult> results = [];
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
                DomclickCatalogueFetchResult fetchResult = new DomclickCatalogueFetchResult(item);
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
