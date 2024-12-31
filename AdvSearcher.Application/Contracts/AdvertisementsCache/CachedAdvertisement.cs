using AdvSearcher.Core.Entities.Advertisements;

namespace AdvSearcher.Application.Contracts.AdvertisementsCache;

public sealed class CachedAdvertisement
{
    public ulong Id { get; }
    public string ServiceName { get; }

    public CachedAdvertisement(Advertisement advertisement)
    {
        Id = advertisement.Id.Id;
        ServiceName = advertisement.ServiceName.ServiceName;
    }
}

public static class CachedAdvertisementExtensions
{
    public static CachedAdvertisement ToCachedAdvertisement(this Advertisement advertisement) =>
        new CachedAdvertisement(advertisement);
}
