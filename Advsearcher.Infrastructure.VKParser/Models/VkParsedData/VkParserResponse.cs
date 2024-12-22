using AdvSearcher.Application.Abstractions.Parsers;
using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;

namespace Advsearcher.Infrastructure.VKParser.Models.VkParsedData;

internal record VkParserResponse : IParserResponse
{
    public IParsedAdvertisement Advertisement { get; }
    public IParsedAttachment[] Attachments { get; }
    public IParsedPublisher Publisher { get; }

    private VkParserResponse(
        IParsedAdvertisement advertisement,
        IParsedAttachment[] attachments,
        IParsedPublisher publisher
    )
    {
        Advertisement = advertisement;
        Attachments = attachments;
        Publisher = publisher;
    }

    internal static VkParserResponse Create(
        VkAdvertisement advertisement,
        VkPublisher publisher,
        VkAttachment[] attachments
    )
    {
        IParsedAdvertisement ad = advertisement;
        IParsedPublisher pub = publisher;
        IParsedAttachment[] photos = attachments;
        return new VkParserResponse(ad, photos, pub);
    }
}
