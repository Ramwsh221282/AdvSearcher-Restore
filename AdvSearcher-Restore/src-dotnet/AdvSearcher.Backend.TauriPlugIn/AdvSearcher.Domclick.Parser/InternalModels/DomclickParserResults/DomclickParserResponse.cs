using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Domclick.Parser.InternalModels.DomclickParserResults;

public sealed record DomclickParserResponse(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse
{
    public string ServiceName { get; } = "DOMCLICK";
}
