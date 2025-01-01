using System.Reflection;
using AdvSearcher.Application.Contracts.AdvertisementsCache;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK;

public static class PersistanceSDKServices
{
    private static readonly string PersistancePath =
        $@"{AppDomain.CurrentDomain.BaseDirectory}\Plugins\Persistance";

    private static readonly string CachePath =
        $@"{AppDomain.CurrentDomain.BaseDirectory}\Plugins\Cache";

    public static IServiceCollection AddPersistanceSDK(this IServiceCollection services)
    {
        services = services.AddPersistance().AddCache();
        return services;
    }

    private static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        if (!Directory.Exists(PersistancePath))
            return services;
        Assembly[] assemblies = Directory
            .GetFiles(PersistancePath, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();
        services = services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IRepositoryPluginInitializer>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
        );
        var provider = services.BuildServiceProvider();
        using (var scope = provider.CreateScope())
        {
            IEnumerable<IRepositoryPluginInitializer> persistancePlugins =
                scope.ServiceProvider.GetServices<IRepositoryPluginInitializer>();
            foreach (var plugin in persistancePlugins)
            {
                services = plugin.Modify(services);
            }
        }
        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services)
    {
        if (!Directory.Exists(CachePath))
            return services;
        Assembly[] assemblies = Directory
            .GetFiles(CachePath, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();
        services = services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes =>
                    classes.AssignableTo<ICachedAdvertisementsPluginDiInitializer>()
                )
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
        );
        var provider = services.BuildServiceProvider();
        using (var scope = provider.CreateScope())
        {
            IEnumerable<ICachedAdvertisementsPluginDiInitializer> plugins =
                scope.ServiceProvider.GetServices<ICachedAdvertisementsPluginDiInitializer>();
            foreach (var plugin in plugins)
            {
                services = plugin.Modify(services);
            }
        }
        return services;
    }
}
