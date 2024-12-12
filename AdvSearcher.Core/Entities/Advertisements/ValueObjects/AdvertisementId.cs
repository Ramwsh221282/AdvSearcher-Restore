using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public readonly record struct AdvertisementId
{
    public ulong Id { get; init; }

    private AdvertisementId(ulong id) => Id = id;

    public static Result<AdvertisementId> Create(string? parsedId)
    {
        if (string.IsNullOrWhiteSpace(parsedId))
            return AdvertisementErrors.EmptyId;

        var canParse = ulong.TryParse(parsedId, out var id);
        if (!canParse)
            return AdvertisementErrors.InvalidId;

        return new AdvertisementId(id);
    }
};
