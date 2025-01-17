namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public readonly record struct AdvertisementId
{
    public ulong Id { get; init; }

    internal AdvertisementId(ulong id) => Id = id; // EF Core Constructor;

    public static AdvertisementId Create(ulong id) => new AdvertisementId(id);
}
