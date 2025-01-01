using AdvSearcher.Application.Contracts.AdvertisementsCache;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.AdvertisementsCache;

public class CacheDiServices : ICachedAdvertisementsPluginDiInitializer
{
    public IServiceCollection Modify(IServiceCollection services)
    {
        services = services
            .AddSingleton<LiteDatabase>()
            .AddScoped<ICachedAdvertisementsRepository, CachedAdvertisements>();
        return services;
    }
}
