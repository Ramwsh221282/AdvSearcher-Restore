using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.Core.Entities.ServiceUrls;

public sealed class ServiceUrl
{
    public delegate Task<Result<ServiceUrl>> ServiceUrlCreatedHandler(ServiceUrl serviceUrl);
    private event ServiceUrlCreatedHandler? OnServiceUrlCreate;

    public int Id { get; init; }
    public ServiceUrlValue Url { get; init; }
    public ServiceUrlMode Mode { get; init; }

    private ServiceUrl(ServiceUrlValue url, ServiceUrlMode mode)
    {
        Url = url;
        Mode = mode;
    }

    public static ServiceUrl Create(ServiceUrlValue url, ServiceUrlMode mode)
    {
        var serviceUrl = new ServiceUrl(url, mode);
        return serviceUrl;
    }

    public static async Task<Result<ServiceUrl>> Create(
        ServiceUrlValue url,
        ServiceUrlMode mode,
        ServiceUrlCreatedHandler handler
    )
    {
        var serviceUrl = Create(url, mode);
        serviceUrl.OnServiceUrlCreate += handler;
        var result = await serviceUrl.OnServiceUrlCreate!.Invoke(serviceUrl);
        serviceUrl.OnServiceUrlCreate -= handler;
        return result;
    }
}
