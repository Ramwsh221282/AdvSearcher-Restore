using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.AdvertisementAttachments;

public class AdvertisementAttachment
{
    public delegate Task<Result<AdvertisementAttachment>> OnAdvertisementAttachedHandler(
        AdvertisementAttachment attachment
    );
    private event OnAdvertisementAttachedHandler? OnAdvertisementAttached;

    public int Id { get; init; }
    public string Url { get; init; }
    public Advertisement Advertisement { get; init; }

    internal AdvertisementAttachment(string url, Advertisement advertisement)
    {
        Url = url;
        Advertisement = advertisement;
    }

    internal static AdvertisementAttachment Create(string url, Advertisement advertisement) =>
        new(url, advertisement);

    internal static async Task<Result<AdvertisementAttachment>> Create(
        string url,
        Advertisement advertisement,
        OnAdvertisementAttachedHandler handler
    )
    {
        var attachment = Create(url, advertisement);
        attachment.OnAdvertisementAttached += handler;
        var result = await attachment.OnAdvertisementAttached.Invoke(attachment);
        attachment.OnAdvertisementAttached -= handler;
        return result;
    }
}
