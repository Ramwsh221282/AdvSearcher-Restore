using AdvSearcher.Application.Abstractions.Parsers;

namespace AdvSearcher.Infrastructure.CianParser.Models.ExternalModels;

public record CianParserResult(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse { }
