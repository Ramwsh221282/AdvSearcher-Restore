using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK;

public static class PersistanceSDKServices
{
    private static readonly string PersistancePath =
        $@"{Environment.CurrentDirectory}\Plugins\Persistance";

    private static readonly string CachePath = $@"{Environment.CurrentDirectory}\Plugins\Cache";

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
        IServiceProvider provider = services.BuildServiceProvider();
        IEnumerable<IRepositoryPluginInitializer> initializers =
            provider.GetServices<IRepositoryPluginInitializer>();
        if (!initializers.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR. Persistance services were not loaded.");
            throw new ApplicationException();
        }
        foreach (IRepositoryPluginInitializer initializer in initializers)
            initializer.Modify(services);
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
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IEnumerable<ICachedAdvertisementsPluginDiInitializer> initializers =
            serviceProvider.GetServices<ICachedAdvertisementsPluginDiInitializer>();
        if (!initializers.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error. Cache service were not loaded.");
            throw new ApplicationException();
        }
        foreach (ICachedAdvertisementsPluginDiInitializer initializer in initializers)
            initializer.AddCache(services);
        return services;
    }
}
