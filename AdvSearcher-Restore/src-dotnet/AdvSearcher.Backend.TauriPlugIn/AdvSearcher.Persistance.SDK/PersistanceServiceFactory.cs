using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK;

public sealed class PersistanceContext : IDisposable
{
    public IServiceScope Scope { get; init; }

    public PersistanceContext(IServiceScope scope) => Scope = scope;

    public void Dispose() => Scope.Dispose();
}

public sealed class PersistanceServiceFactory
{
    private readonly IServiceScopeFactory _factory;

    public PersistanceServiceFactory(IServiceScopeFactory factory) => _factory = factory;

    public PersistanceContext CreateContext() => new PersistanceContext(_factory.CreateScope());

    public IAdvertisementsRepository CreateAdvertisementsRepository(PersistanceContext context) =>
        context.Scope.ServiceProvider.GetRequiredService<IAdvertisementsRepository>();

    public IServiceUrlRepository CreateServiceUrlRepository(PersistanceContext context) =>
        context.Scope.ServiceProvider.GetRequiredService<IServiceUrlRepository>();

    public ICachedAdvertisementsRepository CreateCachedAdvertisementsRepository(
        PersistanceContext context
    ) => context.Scope.ServiceProvider.GetRequiredService<ICachedAdvertisementsRepository>();

    public IPublishersRepository CreatePublishersRepository(PersistanceContext context) =>
        context.Scope.ServiceProvider.GetRequiredService<IPublishersRepository>();
}

public static class PersistanceServiceFactoryExtensions
{
    public static async Task<IEnumerable<ServiceUrl>> GetAllLoadableServiceUrlsOfService(
        this PersistanceServiceFactory factory,
        string serviceName
    )
    {
        using PersistanceContext context = factory.CreateContext();
        IServiceUrlRepository repository = factory.CreateServiceUrlRepository(context);
        return await repository.Get(ServiceUrlMode.Loadable, ServiceUrlService.Create(serviceName));
    }

    public static async Task<IEnumerable<ServiceUrl>> GetAllUploadableServiceUrlsOfService(
        this PersistanceServiceFactory factory,
        string serviceName
    )
    {
        using PersistanceContext context = factory.CreateContext();
        IServiceUrlRepository repository = factory.CreateServiceUrlRepository(context);
        return await repository.Get(
            ServiceUrlMode.Publicatable,
            ServiceUrlService.Create(serviceName)
        );
    }

    public static async Task AppendAdvertisementsCollectionInRepository(
        this PersistanceServiceFactory factory,
        IEnumerable<Advertisement> advertisements
    )
    {
        using PersistanceContext context = factory.CreateContext();
        IAdvertisementsRepository repository = factory.CreateAdvertisementsRepository(context);
        foreach (Advertisement advertisement in advertisements)
            await repository.Add(advertisement);
    }

    public static async Task<Result<RepositoryOperationResult>> AppendServiceUrl(
        this PersistanceServiceFactory factory,
        ServiceUrl url
    )
    {
        using PersistanceContext context = factory.CreateContext();
        IServiceUrlRepository repository = factory.CreateServiceUrlRepository(context);
        return await repository.Add(url);
    }

    public static async Task<Result<RepositoryOperationResult>> RemoveServiceUrl(
        this PersistanceServiceFactory factory,
        ServiceUrl url
    )
    {
        using PersistanceContext context = factory.CreateContext();
        IServiceUrlRepository repository = factory.CreateServiceUrlRepository(context);
        return await repository.Remove(url);
    }
}
