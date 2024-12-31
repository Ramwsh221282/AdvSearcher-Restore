using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public record AdvertisementServiceName
{
    public string ServiceName { get; init; }

    private AdvertisementServiceName(string serviceName) => ServiceName = serviceName;

    public static Result<AdvertisementServiceName> Create(string? serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
            return AdvertisementErrors.ServiceNameEmpty;
        return new AdvertisementServiceName(serviceName);
    }
}
