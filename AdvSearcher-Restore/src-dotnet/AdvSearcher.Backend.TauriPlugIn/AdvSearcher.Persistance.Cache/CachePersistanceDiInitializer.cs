using AdvSearcher.Application.Contracts.AdvertisementCache;
using AdvSearcher.Persistance.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.Cache;

public sealed class CachePersistanceDiInitializer : ICachedAdvertisementsPluginDiInitializer
{
    public IServiceCollection AddCache(IServiceCollection services)
    {
        services = services.AddSingleton<ICachedAdvertisementsRepository, CachedAdvertisements>();
        return services;
    }
}
