using System.Collections;
using AdvSearcher.Core.Tools;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.Avito.Parser.Steps.SecondStep.Models;

public sealed class CatalogueItemsJsonContainer : IEnumerable<JToken>
{
    private readonly JArray _items;

    private CatalogueItemsJsonContainer(JArray items) => _items = items;

    public static Result<CatalogueItemsJsonContainer> Create(JObject json)
    {
        JToken? result = json["result"];
        if (result == null)
            return new Error("Result is null");
        JToken? items = result["items"];
        if (items == null)
            return new Error("Items were null");
        return new CatalogueItemsJsonContainer((JArray)items);
    }

    public IEnumerator<JToken> GetEnumerator()
    {
        foreach (JToken item in _items)
            yield return item;
    }

    public CatalogueItemJson[] CreateCatalogueItemsArray()
    {
        List<CatalogueItemJson> items = [];
        foreach (var token in this)
        {
            JToken? type = token["type"];
            if (type == null)
                continue;
            string typeName = type.ToString();
            if (typeName == "item")
                items.Add(new CatalogueItemJsonBuilder(token).Build());
        }
        return items.ToArray();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
