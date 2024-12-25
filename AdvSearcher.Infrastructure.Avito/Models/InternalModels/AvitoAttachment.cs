using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;

namespace AdvSearcher.Infrastructure.Avito.Models.InternalModels;

internal sealed record AvitoAttachment : IParsedAttachment
{
    public string Url { get; init; }

    private AvitoAttachment(string url) => Url = url;

    public static Result<IParsedAttachment> Create(string? url) =>
        string.IsNullOrWhiteSpace(url)
            ? ParserErrors.CantParseAttachmentUrl
            : new AvitoAttachment(url);

    public static Result<IParsedAttachment[]> Create(AvitoCatalogueItem item)
    {
        List<IParsedAttachment> attachments = [];
        foreach (var url in item.PhotoUrls)
        {
            Result<IParsedAttachment> attachment = Create(url);
            if (!attachment.IsFailure)
                attachments.Add(attachment.Value);
        }
        return attachments.ToArray();
    }
}
