using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;

namespace AdvSearcher.Core.Entities.ServiceUrls;

public sealed class ServiceUrl
{
    public ServiceUrlId Id { get; init; }
    public ServiceUrlValue Value { get; init; }
    public ServiceUrlMode Mode { get; init; }
    public ServiceUrlService Service { get; init; }

    public ServiceUrl(ServiceUrlValue value, ServiceUrlMode mode, ServiceUrlService service)
    {
        Value = value;
        Mode = mode;
        Service = service;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (obj is not ServiceUrl serviceUrl)
            return false;
        return this.Value == serviceUrl.Value
            && this.Mode == serviceUrl.Mode
            && serviceUrl.Service == this.Service;
    }

    public override int GetHashCode() => HashCode.Combine(Id, Value, Mode);
}
