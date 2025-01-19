using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

public sealed record CreatePublishingLinkRequest(string ServiceName, string Link);

public sealed record RemovePublishingLinkRequest(string ServiceName, string Link);

public sealed record PublishingLinksResponse(string ServiceName, string Link);

public sealed record GetPublishingLinksRequest(string ServiceName);

public class PublishingLinksController
{
    private const string Listener = "publishing-links-listener";
    private readonly IEventPublisher _publisher;
    private readonly PersistanceServiceFactory _factory;

    public PublishingLinksController(IEventPublisher publisher, PersistanceServiceFactory factory)
    {
        _publisher = publisher;
        _factory = factory;
    }

    public void CreatePublishingLink(CreatePublishingLinkRequest request)
    {
        try
        {
            ServiceUrlMode mode = ServiceUrlMode.Publicatable;
            Result<ServiceUrlService> service = ServiceUrlService.Create(request.ServiceName);
            if (service.IsFailure)
            {
                _publisher.Publish(Listener, service.Error.Description);
                return;
            }
            Result<ServiceUrlValue> value = ServiceUrlValue.Create(request.Link);
            if (value.IsFailure)
            {
                _publisher.Publish(Listener, service.Error.Description);
                return;
            }
            ServiceUrl url = new ServiceUrl(value, mode, service);
            Result<RepositoryOperationResult> result = _factory.AppendServiceUrl(url).Result;
            if (result.IsFailure)
                _publisher.Publish(Listener, service.Error.Description);
            else
                _publisher.Publish(Listener, "Ссылка публикации добавлена");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }

    public void RemovePublishingLink(RemovePublishingLinkRequest request)
    {
        try
        {
            ServiceUrlMode mode = ServiceUrlMode.Publicatable;
            Result<ServiceUrlService> service = ServiceUrlService.Create(request.ServiceName);
            if (service.IsFailure)
            {
                _publisher.Publish(Listener, service.Error.Description);
                return;
            }
            Result<ServiceUrlValue> value = ServiceUrlValue.Create(request.Link);
            if (value.IsFailure)
            {
                _publisher.Publish(Listener, service.Error.Description);
                return;
            }
            ServiceUrl url = new ServiceUrl(value, mode, service);
            Result<RepositoryOperationResult> result = _factory.RemoveServiceUrl(url).Result;
            if (result.IsFailure)
                _publisher.Publish(Listener, service.Error.Description);
            else
                _publisher.Publish(Listener, "Ссылка публикации добавлена");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }

    public IEnumerable<PublishingLinksResponse> GetPublishingLinksOfService(
        GetPublishingLinksRequest request
    )
    {
        try
        {
            IEnumerable<ServiceUrl> urls = _factory
                .GetAllUploadableServiceUrlsOfService(request.ServiceName)
                .Result;
            return urls.Select(u => new PublishingLinksResponse(u.Service.Name, u.Value.Value));
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
            return [];
        }
    }
}
