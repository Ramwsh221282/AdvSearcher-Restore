using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Infrastracture.OkParser.Models.InternalModels;

namespace AdvSearcher.Infrastracture.OkParser.Models.ExternalModels;

public sealed record OkParserResults : IParserResponse
{
    public IParsedAdvertisement Advertisement { get; init; }
    public IParsedAttachment[] Attachments { get; init; }
    public IParsedPublisher Publisher { get; init; }

    internal OkParserResults(
        IParsedAdvertisement advertisement,
        IParsedAttachment[] attachments,
        IParsedPublisher publisher
    )
    {
        Advertisement = advertisement;
        Attachments = attachments;
        Publisher = publisher;
    }

    internal static OkParserResults Create(
        OkParsedAdvertisement advertisement,
        List<OkParsedAttachment> attachments,
        OkParsedPublisher publisher
    )
    {
        IParsedAdvertisement ad = advertisement;
        IParsedPublisher pub = publisher;
        var photos = new IParsedAttachment[attachments.Count];
        for (var index = 0; index < attachments.Count; index++)
        {
            photos[index] = attachments[index];
        }

        return new OkParserResults(ad, photos, pub);
    }
}
