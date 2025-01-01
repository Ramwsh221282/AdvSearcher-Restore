using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.Application.Contracts.AdvertisementsCache;

public sealed record CachedAdvertisement
{
    public string Key { get; }

    public CachedAdvertisement(Advertisement advertisement)
    {
        string key = $"{advertisement.ServiceName.ServiceName}_{advertisement.Id.Id}";
        Key = key;
    }
}

public static class CachedAdvertisementExtensions
{
    public static CachedAdvertisement ToCachedAdvertisement(this Advertisement advertisement) =>
        new CachedAdvertisement(advertisement);

    public static string GetServiceName(this CachedAdvertisement cachedAdvertisement)
    {
        ReadOnlySpan<string> span = cachedAdvertisement.Key.Split('_');
        return span[0];
    }

    public static string GetId(this CachedAdvertisement cachedAdvertisement)
    {
        ReadOnlySpan<string> span = cachedAdvertisement.Key.Split('_');
        return span[^1];
    }
}
