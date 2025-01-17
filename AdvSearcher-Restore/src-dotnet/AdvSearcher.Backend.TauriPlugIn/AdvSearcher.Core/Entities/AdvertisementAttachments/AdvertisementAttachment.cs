using AdvSearcher.Core.Entities.AdvertisementAttachments.ValueObjects;
using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.Core.Entities.AdvertisementAttachments;

public sealed class AdvertisementAttachment
{
    public AdvertisementAttachmentId Id { get; init; }
    public AdvertisementAttachmentSourceUrl Url { get; init; }
    public Advertisement Advertisement { get; init; }

    private AdvertisementAttachment() { } // EF Core Constructor

    public AdvertisementAttachment(
        AdvertisementAttachmentSourceUrl url,
        Advertisement advertisement
    ) => (Url, Advertisement) = (url, advertisement);
}
