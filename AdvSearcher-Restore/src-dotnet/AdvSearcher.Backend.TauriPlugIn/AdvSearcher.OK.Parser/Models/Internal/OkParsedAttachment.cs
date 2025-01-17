using AdvSearcher.Core.Entities.AdvertisementAttachments.Errors;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.OK.Parser.Models.Internal;

public sealed record OkParsedAttachment : IParsedAttachment
{
    public string Url { get; init; }

    private OkParsedAttachment(string url) => Url = url;

    public static Result<IParsedAttachment> Create(string? url) =>
        string.IsNullOrWhiteSpace(url)
            ? AdvertisementAttachmentErrors.UrlEmpty
            : new OkParsedAttachment(url);
}
