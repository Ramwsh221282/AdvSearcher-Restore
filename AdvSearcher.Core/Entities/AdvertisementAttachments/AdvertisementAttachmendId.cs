namespace AdvSearcher.Core.Entities.AdvertisementAttachments;

public readonly record struct AdvertisementAttachmendId
{
    public int Value { get; }

    internal AdvertisementAttachmendId(int value) => Value = value;

    internal static AdvertisementAttachmendId CreateEmpty() => new AdvertisementAttachmendId(0);

    public static AdvertisementAttachmendId Create(int value) =>
        new AdvertisementAttachmendId(value);
}
