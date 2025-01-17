using AdvSearcher.Core.Entities.AdvertisementAttachments.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.AdvertisementAttachments.ValueObjects;

public sealed record AdvertisementAttachmentSourceUrl
{
    public string Value { get; init; } = string.Empty;

    private AdvertisementAttachmentSourceUrl() { } // ef core constructor

    private AdvertisementAttachmentSourceUrl(string value) => Value = value;

    public static Result<AdvertisementAttachmentSourceUrl> Create(string? value) =>
        value switch
        {
            null => AdvertisementAttachmentErrors.UrlEmpty,
            not null when string.IsNullOrWhiteSpace(value) =>
                AdvertisementAttachmentErrors.UrlEmpty,
            _ => new AdvertisementAttachmentSourceUrl(value),
        };
}
