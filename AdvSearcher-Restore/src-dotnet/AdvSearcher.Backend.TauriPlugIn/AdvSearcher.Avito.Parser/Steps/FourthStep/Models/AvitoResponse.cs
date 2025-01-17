using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Avito.Parser.Steps.FourthStep.Models;

public sealed record AvitoResponse(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse
{
    public string ServiceName { get; } = "AVITO";
}
