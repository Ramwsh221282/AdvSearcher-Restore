using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Infrastracture.OkParser.Models.InternalModels;

internal sealed record OkParsedAttachment : IParsedAttachment
{
    public string Url { get; init; }

    private OkParsedAttachment(string url) => Url = url;

    public static Result<IParsedAttachment> Create(string? url) =>
        string.IsNullOrWhiteSpace(url)
            ? AdvertisementAttachmentErrors.UrlEmpty
            : new OkParsedAttachment(url);
}
