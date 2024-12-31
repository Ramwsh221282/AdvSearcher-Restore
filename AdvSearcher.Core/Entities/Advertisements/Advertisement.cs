using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Entities.Publishers;

namespace AdvSearcher.Core.Entities.Advertisements;

public sealed class Advertisement
{
    private readonly List<AdvertisementAttachment> _attachments = [];

    public AdvertisementId Id { get; init; }
    public AdvertisementContent Content { get; init; }
    public AdvertisementPublishedDate Date { get; init; }
    public AdvertisementServiceName ServiceName { get; init; }
    public AdvertisementUrl Url { get; init; }
    public CreationDate CreationDate { get; init; }
    public Publisher? Publisher { get; set; }

    public IReadOnlyCollection<AdvertisementAttachment> Attachments => _attachments;

    public Advertisement(
        AdvertisementId id,
        AdvertisementContent content,
        AdvertisementPublishedDate date,
        AdvertisementServiceName serviceName,
        AdvertisementUrl url,
        CreationDate creationDate,
        List<AdvertisementAttachment>? attachments = null
    )
    {
        Id = id;
        Content = content;
        Date = date;
        ServiceName = serviceName;
        Url = url;
        CreationDate = creationDate;
        if (attachments != null)
            _attachments = attachments;
    }
}
