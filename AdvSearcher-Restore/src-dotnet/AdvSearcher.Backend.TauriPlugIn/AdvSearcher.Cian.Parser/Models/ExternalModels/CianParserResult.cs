using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Cian.Parser.Models.ExternalModels;

public record CianParserResult(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse
{
    public string ServiceName { get; } = "CIAN";
}
