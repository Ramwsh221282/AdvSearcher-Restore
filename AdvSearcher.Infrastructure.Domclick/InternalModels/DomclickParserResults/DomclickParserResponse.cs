using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Domclick.InternalModels.DomclickParserResults;

internal sealed record DomclickParserResponse(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse
{
    public string ServiceName { get; } = "DOMCLICK";
}
