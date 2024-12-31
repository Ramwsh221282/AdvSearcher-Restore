namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public readonly record struct AdvertisementId
{
    public ulong Id { get; init; }

    private AdvertisementId(ulong id) => Id = id;

    public static AdvertisementId Create(ulong id)
    {
        return new AdvertisementId(id);
    }
};
