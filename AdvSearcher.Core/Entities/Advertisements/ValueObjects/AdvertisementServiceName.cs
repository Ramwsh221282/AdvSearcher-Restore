using AdvSearcher.Core.Entities.Advertisements.Errors;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.Advertisements.ValueObjects;

public record AdvertisementServiceName
{
    public static readonly AdvertisementServiceName VK = new AdvertisementServiceName("ВК");
    public static readonly AdvertisementServiceName OK = new AdvertisementServiceName("ОК");
    public static readonly AdvertisementServiceName Cian = new AdvertisementServiceName("Cian");
    public static readonly AdvertisementServiceName Avito = new AdvertisementServiceName("Avito");
    public static readonly AdvertisementServiceName Domclick = new AdvertisementServiceName(
        "Domclick"
    );

    public string ServiceName { get; init; }

    private AdvertisementServiceName(string serviceName) => ServiceName = serviceName;

    public static Result<AdvertisementServiceName> Create(string? serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
            return AdvertisementErrors.ServiceNameEmpty;
        return new AdvertisementServiceName(serviceName);
    }
}
