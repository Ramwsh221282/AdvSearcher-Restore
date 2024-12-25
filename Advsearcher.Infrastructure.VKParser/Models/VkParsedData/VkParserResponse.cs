using AdvSearcher.Application.Abstractions.Parsers;

namespace Advsearcher.Infrastructure.VKParser.Models.VkParsedData;

internal record VkParserResponse : IParserResponse
{
    public IParsedAdvertisement Advertisement { get; }
    public IParsedAttachment[] Attachments { get; }
    public IParsedPublisher Publisher { get; }

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
