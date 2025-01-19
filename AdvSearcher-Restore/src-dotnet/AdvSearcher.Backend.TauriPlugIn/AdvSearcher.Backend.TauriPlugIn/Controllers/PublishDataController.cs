using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Persistance.SDK;
using AdvSearcher.Publishing.SDK.DependencyInjection;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public class PublishDataController
{
    private const string MaxProgressListener = "publishing-max-progress";
    private const string CurrentProgressListener = "publishing-current-progress";
    private readonly PersistanceServiceFactory _factory;
    private readonly PublishingServiceProvider _provider;
    private readonly IEventPublisher _publisher;

    public PublishDataController(
        PersistanceServiceFactory factory,
        PublishingServiceProvider provider,
        IEventPublisher publisher
    )
    {
        _factory = factory;
        _provider = provider;
        _publisher = publisher;
    }

    public void PublishToSocialMedia(SocialMediaPublishRequest request)
    {
        try
        {
            string serviceName = request.ServiceName;
            IEnumerable<ServiceUrl> urls = serviceName switch
            {
                null => [],
                not null when string.IsNullOrWhiteSpace(serviceName) => [],
                not null
                    when serviceName.Contains("VK", StringComparison.CurrentCultureIgnoreCase) =>
                    _factory.GetAllUploadableServiceUrlsOfService("VK").Result,
                not null when serviceName.Contains("OK", StringComparison.OrdinalIgnoreCase) =>
                    _factory.GetAllUploadableServiceUrlsOfService("OK").Result,
                _ => [],
            };
            Action<int> currentProgress = (value) =>
                _publisher.Publish(CurrentProgressListener, value);
            Action<int> maxProgress = (value) => _publisher.Publish(MaxProgressListener, value);
            foreach (ServiceUrl url in urls)
            {
                request.Handle(_provider, url, currentProgress, maxProgress).Wait();
                currentProgress.Invoke(0);
                maxProgress.Invoke(0);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }

    // email services don't require service Urls;
    public void PublishToEmail(EmailPublishRequest request)
    {
        try
        {
            Action<int> currentProgress = (value) =>
                _publisher.Publish(CurrentProgressListener, value);
            Action<int> maxProgress = (value) => _publisher.Publish(MaxProgressListener, value);
            request.Handle(_provider, null!, currentProgress, maxProgress).Wait();
            currentProgress.Invoke(0);
            maxProgress.Invoke(0);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }

    // whats app services don't require service Urls;
    public void PublishToWhatsApp(WhatsAppPublishRequest request)
    {
        try
        {
            Action<int> currentProgress = (value) =>
                _publisher.Publish(CurrentProgressListener, value);
            Action<int> maxProgress = (value) => _publisher.Publish(MaxProgressListener, value);
            request.Handle(_provider, null!, currentProgress, maxProgress).Wait();
            currentProgress.Invoke(0);
            maxProgress.Invoke(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }
}
