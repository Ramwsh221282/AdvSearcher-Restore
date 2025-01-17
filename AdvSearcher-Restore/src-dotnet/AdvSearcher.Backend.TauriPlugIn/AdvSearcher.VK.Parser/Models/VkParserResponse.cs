using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.VK.Parser.Models;

public record VkParserResponse : IParserResponse
{
    public IParsedAdvertisement Advertisement { get; }
    public IParsedAttachment[] Attachments { get; }
    public IParsedPublisher Publisher { get; }
    public string ServiceName { get; } = "VK";

    public VkParserResponse(
        IParsedAdvertisement advertisement,
        IParsedAttachment[] attachments,
        IParsedPublisher publisher
    )
    {
        Advertisement = advertisement;
        Attachments = attachments;
        Publisher = publisher;
    }
}
