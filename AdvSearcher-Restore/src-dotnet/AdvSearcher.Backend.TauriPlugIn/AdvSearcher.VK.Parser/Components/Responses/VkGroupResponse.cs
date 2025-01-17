using Newtonsoft.Json.Linq;

namespace AdvSearcher.VK.Parser.Components.Responses;

public sealed class VkGroupResponse(string response)
{
    public JToken[] Items { get; init; } = JObject.Parse(response)["response"]!.ToArray();
}
