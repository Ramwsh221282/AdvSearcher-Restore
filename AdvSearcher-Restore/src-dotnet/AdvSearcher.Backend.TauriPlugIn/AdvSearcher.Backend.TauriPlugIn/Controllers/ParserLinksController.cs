using AdvSearcher.Backend.TauriPlugIn.CommonComponents;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Persistance.SDK;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

// create link request
public record LinkRequest(ParserService Parser, ParserLink Link);

// parser service argument
public record ParserService(string ServiceName);

// parser link argument
public record ParserLink(string Link);

// response argument
public record ParserLinksResponse(List<string> Links);

// controller for loadable service url manipulations
public class ParserLinksController(PersistanceServiceFactory factory, IEventPublisher publisher)
{
    // tauri frontend listener
    private const string AddLinkListenerName = "parser-link-listener";

    // repository factory reference
    private readonly PersistanceServiceFactory _factory = factory;

    // tauri frontend publisher
    private readonly IEventPublisher _publisher = publisher;

    // method to add loadable service url
    public void AddLink(LinkRequest request)
    {
        using PersistanceContext context = _factory.CreateContext();
        IServiceUrlRepository repository = _factory.CreateServiceUrlRepository(context);
        _publisher.Publish(AddLinkListenerName, "Проверка входных данных");
        ResultPipeline pipeline = new ResultPipeline(_publisher, AddLinkListenerName);
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        Result<ServiceUrlValue> value = pipeline.Do(
            () => ServiceUrlValue.Create(request.Link.Link)
        );
        Result<ServiceUrlService> service = pipeline.Do(
            () => ServiceUrlService.Create(request.Parser.ServiceName)
        );
        Result<ServiceUrl> url = pipeline.Do(() => ServiceUrl.Create(value, service, mode));
        if (url.IsFailure)
            return;
        _publisher.Publish(AddLinkListenerName, "Добавление в базу данных");
        Result<RepositoryOperationResult> result = pipeline.Do(() => repository.Add(url).Result);
        if (result.IsFailure)
            return;
        _publisher.Publish(AddLinkListenerName, "Ссылка успешно добавлена!");
    }

    // method to get loadable service urls
    public IEnumerable<string> GetLinks(ParserService parser)
    {
        using PersistanceContext context = _factory.CreateContext();
        IServiceUrlRepository repository = _factory.CreateServiceUrlRepository(context);
        ResultPipeline pipeline = new ResultPipeline(_publisher, AddLinkListenerName);
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        Result<ServiceUrlService> service = pipeline.Do(
            () => ServiceUrlService.Create(parser.ServiceName)
        );
        if (service.IsFailure)
            return [];
        IEnumerable<ServiceUrl> urls = repository.Get(mode, service).Result;
        return (urls.Select(u => u.Value.Value));
    }

    // method to remove service url
    public void RemoveLink(LinkRequest request)
    {
        using PersistanceContext context = _factory.CreateContext();
        IServiceUrlRepository repository = _factory.CreateServiceUrlRepository(context);
        ResultPipeline pipeline = new ResultPipeline(_publisher, AddLinkListenerName);
        ServiceUrlMode mode = ServiceUrlMode.Loadable;
        Result<ServiceUrlValue> value = pipeline.Do(
            () => ServiceUrlValue.Create(request.Link.Link)
        );
        Result<ServiceUrlService> service = pipeline.Do(
            () => ServiceUrlService.Create(request.Parser.ServiceName)
        );
        Result<ServiceUrl> url = pipeline.Do(() => ServiceUrl.Create(value, service, mode));
        Result<RepositoryOperationResult> result = pipeline.Do(() => repository.Remove(url).Result);
        if (result.IsFailure)
            return;
        _publisher.Publish(AddLinkListenerName, "Ссылка была успешно удалена!");
    }
}
