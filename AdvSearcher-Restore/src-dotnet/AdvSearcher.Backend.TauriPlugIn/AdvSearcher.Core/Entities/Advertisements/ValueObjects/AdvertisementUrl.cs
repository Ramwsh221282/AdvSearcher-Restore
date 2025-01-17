using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public sealed record AdvertisementUrl
{
    public string Url { get; init; }

    private AdvertisementUrl() => Url = string.Empty; // EF Core Constructor

    private AdvertisementUrl(string url) => Url = url;

    public static Result<AdvertisementUrl> Create(string? url) =>
        url switch
        {
            null => AdvertisementErrors.EmptyUrl,
            not null when string.IsNullOrWhiteSpace(url) => AdvertisementErrors.EmptyUrl,
            _ => new AdvertisementUrl(url),
        };
}
