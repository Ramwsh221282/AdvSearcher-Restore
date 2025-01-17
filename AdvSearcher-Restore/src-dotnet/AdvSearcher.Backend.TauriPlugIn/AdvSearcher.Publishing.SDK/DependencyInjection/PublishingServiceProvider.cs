using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Publishing.SDK.DependencyInjection;

public sealed class PublishingServiceProvider
{
    private readonly IServiceProvider _provider;

    public PublishingServiceProvider(IServiceProvider provider) => _provider = provider;

    public IPublishingService GetService(string serviceName)
    {
        IEnumerable<IPublishingService> services = _provider.GetServices<IPublishingService>();
        return services.First(svc => svc.GetType().Name == serviceName);
    }

    public IWhatsAppSender GetWhatsAppService(string serviceName)
    {
        IEnumerable<IWhatsAppSender> services = _provider.GetServices<IWhatsAppSender>();
        return services.First(svc => svc.GetType().Name == serviceName);
    }

    public IMailingClient GetMailingClient(string serviceName)
    {
        IEnumerable<IMailingClient> services = _provider.GetServices<IMailingClient>();
        return services.First(svc => svc.GetType().Name == serviceName);
    }
}
