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
}
