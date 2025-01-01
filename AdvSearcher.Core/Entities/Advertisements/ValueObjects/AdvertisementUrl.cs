using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public record AdvertisementUrl
{
    public string Url { get; init; }

    private AdvertisementUrl()
    {
        Url = string.Empty;
    } // ef core constructor

    private AdvertisementUrl(string url) => Url = url;

    public static Result<AdvertisementUrl> Create(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return AdvertisementErrors.EmptyUrl;
        return new AdvertisementUrl(url);
    }
}
