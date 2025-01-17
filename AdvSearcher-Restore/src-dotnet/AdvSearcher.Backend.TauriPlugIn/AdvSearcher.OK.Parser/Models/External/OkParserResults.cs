using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.OK.Parser.Models.External;

public sealed record OkParserResults(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse
{
    public string ServiceName { get; } = "OK";
}
