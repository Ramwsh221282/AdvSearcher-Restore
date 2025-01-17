using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls;

public sealed class ServiceUrl
{
    public ServiceUrlId Id { get; init; }
    public ServiceUrlValue Value { get; init; }
    public ServiceUrlMode Mode { get; init; }
    public ServiceUrlService Service { get; init; }

    private ServiceUrl() { } // EF Core Constructor

    public ServiceUrl(ServiceUrlValue value, ServiceUrlMode mode, ServiceUrlService service) =>
        (Value, Mode, Service) = (value, mode, service);

    public static Result<ServiceUrl> Create(
        Result<ServiceUrlValue> value,
        Result<ServiceUrlService> service,
        ServiceUrlMode mode
    )
    {
        if (value.IsFailure)
            return value.Error;
        if (service.IsFailure)
            return service.Error;
        return new ServiceUrl(value.Value, mode, service.Value);
    }
}
