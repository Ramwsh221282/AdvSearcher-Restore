using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.Core.Entities.AdvertisementAttachments;

public sealed class AdvertisementAttachment
{
    public AdvertisementAttachmendId Id { get; init; }
    public AdvertisementAttachmentSourceUrl Url { get; init; }
    public Advertisement Advertisement { get; init; }

    private AdvertisementAttachment() { } // ef core constructor

    public AdvertisementAttachment(
        AdvertisementAttachmentSourceUrl url,
        Advertisement advertisement
    )
    {
        Id = AdvertisementAttachmendId.CreateEmpty();
        Url = url;
        Advertisement = advertisement;
    }
}
