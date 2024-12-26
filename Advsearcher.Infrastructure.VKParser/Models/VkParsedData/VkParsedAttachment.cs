using AdvSearcher.Parser.SDK.Contracts;

namespace Advsearcher.Infrastructure.VKParser.Models.VkParsedData;

internal sealed record VkParsedAttachment : IParsedAttachment
{
    public string Url { get; init; }

    internal VkParsedAttachment(string url) => Url = url;
}
