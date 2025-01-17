using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public readonly record struct AdvertisementPublishedDate
{
    public DateOnly Value { get; init; }

    private AdvertisementPublishedDate(DateOnly value) => Value = value;

    public AdvertisementPublishedDate() => Value = default;

    public static Result<AdvertisementPublishedDate> Create(DateOnly value) =>
        new AdvertisementPublishedDate(value);
}
