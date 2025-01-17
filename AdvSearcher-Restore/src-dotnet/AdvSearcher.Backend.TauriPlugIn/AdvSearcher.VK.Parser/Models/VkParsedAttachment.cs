using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.VK.Parser.Models;

public sealed record VkParsedAttachment : IParsedAttachment
{
    public string Url { get; init; }

    internal VkParsedAttachment(string url) => Url = url;
}
