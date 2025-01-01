using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Avito.Models.InternalModels;

internal sealed record AvitoParserResult(
    IParsedAdvertisement Advertisement,
    IParsedAttachment[] Attachments,
    IParsedPublisher Publisher
) : IParserResponse
{
    public string ServiceName { get; } = "AVITO";
}
