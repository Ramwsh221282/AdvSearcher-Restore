using AdvSearcher.Parser.SDK.Contracts;

namespace Advsearcher.Infrastructure.VKParser.Models.VkParsedData;

internal sealed record VkParsedPublisher : IParsedPublisher
{
    public string Id { get; init; }
    public string Info { get; set; }

    internal VkParsedPublisher(string id, string name)
    {
        Id = id;
        Info = name;
    }
};
