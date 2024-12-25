using AdvSearcher.Application.Abstractions.Parsers;

namespace AdvSearcher.Infrastructure.Domclick.InternalModels.DomclickParserResults;

internal sealed record DomclickParserResponse(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse;
