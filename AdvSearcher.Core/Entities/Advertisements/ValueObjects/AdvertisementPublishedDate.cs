using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public readonly record struct AdvertisementPublishedDate
{
    public DateOnly Value { get; init; }

    private AdvertisementPublishedDate(DateOnly date) => Value = date;

    public AdvertisementPublishedDate()
    {
        Value = default;
    } // ef core constructor

    public static Result<AdvertisementPublishedDate> Create(DateOnly date) =>
        new AdvertisementPublishedDate(date);
};
