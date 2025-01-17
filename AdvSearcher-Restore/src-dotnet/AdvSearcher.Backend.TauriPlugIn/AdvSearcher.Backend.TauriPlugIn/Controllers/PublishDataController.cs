using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Persistance.SDK;
using AdvSearcher.Publishing.SDK.DependencyInjection;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public class PublishDataController
{
    private const string Listener = "publish-data-listener";
    private readonly PersistanceServiceFactory _factory;
    private readonly PublishingServiceProvider _provider;

    public PublishDataController(
        PersistanceServiceFactory factory,
        PublishingServiceProvider provider
    )
    {
        _factory = factory;
        _provider = provider;
    }

    public void PublishToSocialMedia(SocialMediaPublishRequest request)
    {
        string serviceName = request.ServiceName;
        IEnumerable<ServiceUrl> urls = serviceName switch
        {
            null => [],
            not null when string.IsNullOrWhiteSpace(serviceName) => [],
            not null when serviceName.Contains("VK", StringComparison.CurrentCultureIgnoreCase) =>
                _factory.GetAllUploadableServiceUrlsOfService("VK").Result,
            not null when serviceName.Contains("OK", StringComparison.OrdinalIgnoreCase) => _factory
                .GetAllUploadableServiceUrlsOfService("OK")
                .Result,
            _ => [],
        };
        foreach (ServiceUrl url in urls)
            request.Handle(_provider, url).Wait();
    }

    // email services don't require service Urls;
    public void PublishToEmail(EmailPublishRequest request) =>
        request.Handle(_provider, null!).Wait();

    // whats app services don't require service Urls;
    public void PublishToWhatsApp(WhatsAppPublishRequest request) =>
        request.Handle(_provider, null!).Wait();
}
