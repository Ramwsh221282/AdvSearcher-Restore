using Newtonsoft.Json.Linq;

namespace AdvSearcher.VK.Parser.Components.Responses;

public sealed class VkItemsJson(string response)
{
    public JToken[] Items { get; init; } =
        JObject.Parse(response)["response"]!["items"]!
            .Select(item => item)
            .Where(item => !string.IsNullOrWhiteSpace(item["text"]!.ToString()))
            .ToArray();
}
