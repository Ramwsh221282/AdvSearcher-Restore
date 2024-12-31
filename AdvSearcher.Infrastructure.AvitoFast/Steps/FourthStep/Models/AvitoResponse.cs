using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.AvitoFast.Steps.FourthStep.Models;

internal sealed record AvitoResponse(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse;
