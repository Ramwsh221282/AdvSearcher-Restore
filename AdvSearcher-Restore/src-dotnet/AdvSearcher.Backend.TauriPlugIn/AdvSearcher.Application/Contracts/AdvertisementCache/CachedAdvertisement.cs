using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.Application.Contracts.AdvertisementCache;

public sealed class CachedAdvertisement
{
    public string Key { get; init; }

    public CachedAdvertisement() => Key = string.Empty;

    public CachedAdvertisement(Advertisement advertisement)
    {
        string key = $"{advertisement.ServiceName.ServiceName}_{advertisement.Id.Id}";
        Key = key;
    }

    public string GetId()
    {
        ReadOnlySpan<string> span = Key.Split('_');
        return span[^1];
    }

    public string GetServiceName()
    {
        ReadOnlySpan<string> span = Key.Split('_');
        return span[0];
    }
}

public static class CachedAdvertisementExtensions
{
    public static CachedAdvertisement ToCachedAdvertisement(this Advertisement advertisement) =>
        new CachedAdvertisement(advertisement);
}
