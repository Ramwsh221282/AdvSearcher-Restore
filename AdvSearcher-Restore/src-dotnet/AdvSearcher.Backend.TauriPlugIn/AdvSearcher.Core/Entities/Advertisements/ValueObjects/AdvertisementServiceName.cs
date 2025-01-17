using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public sealed record AdvertisementServiceName
{
    public string ServiceName { get; init; }

    private AdvertisementServiceName() => ServiceName = string.Empty;

    private AdvertisementServiceName(string serviceName) => ServiceName = serviceName;

    public static Result<AdvertisementServiceName> Create(string? serviceName) =>
        serviceName switch
        {
            null => AdvertisementErrors.ServiceNameEmpty,
            not null when string.IsNullOrWhiteSpace(serviceName) =>
                AdvertisementErrors.ServiceNameEmpty,
            _ => new AdvertisementServiceName(serviceName),
        };
}
