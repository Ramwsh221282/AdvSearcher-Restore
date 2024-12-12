using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements;

public sealed class Advertisement
{
    public delegate Task<Result<Advertisement>> OnAdvertisementCreatedHandler(
        Advertisement advertisement
    );
    private event OnAdvertisementCreatedHandler? OnAdvertisementCreated;

    public delegate Task<Result<AdvertisementAttachment>> OnAttachmentAddedHandler(
        AdvertisementAttachment attachment
    );
    public event OnAttachmentAddedHandler? OnAttachmentAdded;

    private readonly List<AdvertisementAttachment> _attachments = [];

    public AdvertisementId Id { get; init; }
    public AdvertisementContent Content { get; init; }
    public AdvertisementPublishedDate Date { get; init; }
    public AdvertisementServiceName ServiceName { get; init; }
    public AdvertisementUrl Url { get; init; }

    public IReadOnlyCollection<AdvertisementAttachment> Attachments => _attachments;

    private Advertisement(
        AdvertisementId id,
        AdvertisementContent content,
        AdvertisementPublishedDate date,
        AdvertisementServiceName serviceName,
        AdvertisementUrl url
    )
    {
        Id = id;
        Content = content;
        Date = date;
        ServiceName = serviceName;
        Url = url;
    }

    public static Advertisement Create(
        AdvertisementId id,
        AdvertisementContent content,
        AdvertisementPublishedDate date,
        AdvertisementServiceName serviceName,
        AdvertisementUrl url
    )
    {
        var advertisement = new Advertisement(id, content, date, serviceName, url);
        return advertisement;
    }

    public static async Task<Result<Advertisement>> Create(
        AdvertisementId id,
        AdvertisementContent content,
        AdvertisementPublishedDate date,
        AdvertisementServiceName serviceName,
        AdvertisementUrl url,
        OnAdvertisementCreatedHandler handler
    )
    {
        var advertisement = Create(id, content, date, serviceName, url);
        advertisement.OnAdvertisementCreated += handler;
        var result = await advertisement.OnAdvertisementCreated!.Invoke(advertisement);
        advertisement.OnAdvertisementCreated -= handler;
        return result;
    }

    public Result<AdvertisementAttachment> AddAttachment(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return AdvertisementErrors.AttachmentUrlEmpty;
        var attachment = new AdvertisementAttachment(url, this);
        _attachments.Add(attachment);
        return attachment;
    }

    public async Task<Result<AdvertisementAttachment>> AddAttachment(
        string? url,
        OnAttachmentAddedHandler handler
    )
    {
        var attachment = AddAttachment(url);
        OnAttachmentAdded += handler;
        var result = await OnAttachmentAdded!.Invoke(attachment.Value);
        OnAttachmentAdded -= handler;
        return result;
    }
}
