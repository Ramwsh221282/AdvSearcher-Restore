using Newtonsoft.Json.Linq;

namespace Advsearcher.Infrastructure.VKParser.Components.Responses;

internal sealed class VkGroupResponse(string response)
{
    public JToken[] Items { get; init; } = JObject.Parse(response)["response"]!.ToArray();
}
