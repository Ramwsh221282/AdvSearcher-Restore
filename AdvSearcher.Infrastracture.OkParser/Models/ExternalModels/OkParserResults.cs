using AdvSearcher.Application.Abstractions.Parsers;

namespace AdvSearcher.Infrastracture.OkParser.Models.ExternalModels;

internal sealed record OkParserResults(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse;
