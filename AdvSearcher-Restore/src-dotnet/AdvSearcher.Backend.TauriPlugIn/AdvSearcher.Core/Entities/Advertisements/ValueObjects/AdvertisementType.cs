using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public sealed record AdvertisementType
{
    public string Type { get; init; }

    private AdvertisementType() { } // EF Core Constructor

    private AdvertisementType(string type) => Type = type;

    public static Result<AdvertisementType> Create(string? type) =>
        type switch
        {
            null => AdvertisementErrors.TypeEmpty,
            not null when string.IsNullOrWhiteSpace(type) => AdvertisementErrors.TypeEmpty,
            _ => new AdvertisementType(type),
        };
}
