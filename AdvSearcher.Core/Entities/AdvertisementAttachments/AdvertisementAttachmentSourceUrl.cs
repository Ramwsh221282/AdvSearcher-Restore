using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.AdvertisementAttachments;

public sealed record AdvertisementAttachmentSourceUrl
{
    public string Value { get; }

    private AdvertisementAttachmentSourceUrl(string value) => Value = value;

    public static Result<AdvertisementAttachmentSourceUrl> Create(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? AdvertisementAttachmentErrors.UrlEmpty
            : new AdvertisementAttachmentSourceUrl(value);
}
