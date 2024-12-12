using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public record AdvertisementType
{
    public string Type { get; init; }

    private AdvertisementType(string type) => Type = type;

    public static Result<AdvertisementType> Create(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return AdvertisementErrors.TypeEmpty;
        return new AdvertisementType(type);
    }
}
