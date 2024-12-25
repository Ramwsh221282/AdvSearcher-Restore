using AdvSearcher.Application.Abstractions.Parsers;

namespace AdvSearcher.Infrastructure.Avito.Models.InternalModels;

internal sealed record AvitoParserResult(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse;
