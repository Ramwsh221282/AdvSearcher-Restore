using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.VK.Parser.Models;

public sealed record VkParsedPublisher : IParsedPublisher
{
    public string Id { get; init; }
    public string Info { get; set; }

    internal VkParsedPublisher(string id, string name)
    {
        Id = id;
        Info = name;
    }
}
