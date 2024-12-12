using Newtonsoft.Json.Linq;

namespace Advsearcher.Infrastructure.VKParser.Components.Responses;

internal sealed class VkItemsJson(string response)
{
    public JToken[] Items { get; init; } =
        JObject.Parse(response)["response"]!["items"]!
            .Select(item => item)
            .Where(item => !string.IsNullOrWhiteSpace(item["text"]!.ToString()))
            .ToArray();
}
