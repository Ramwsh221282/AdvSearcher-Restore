namespace AdvSearcher.Core.Entities.AdvertisementAttachments.ValueObjects;

public readonly record struct AdvertisementAttachmentId
{
    public int Value { get; init; }

    internal AdvertisementAttachmentId(int value) => Value = value;

    public static AdvertisementAttachmentId Create(int value) =>
        new AdvertisementAttachmentId(value);
}
