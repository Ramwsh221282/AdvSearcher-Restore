using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastracture.OkParser.Models.ExternalModels;

internal sealed record OkParserResults(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse
{
    public string ServiceName { get; } = "OK";
}
