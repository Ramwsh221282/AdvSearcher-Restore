using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK;

public static class PersistanceSDKServices
{
    // private static readonly string PersistancePath =
    //     $@"{Environment.CurrentDirectory}\Plugins\Persistance";

    private static readonly string PersistancePath =
        @"D:\AdvSearcher-Restore\AdvSearcher-Restore\src-tauri\Plugins\Persistance";

    //private static readonly string CachePath = $@"{Environment.CurrentDirectory}\Plugins\Cache";

    private static readonly string CachePath =
        @"D:\AdvSearcher-Restore\AdvSearcher-Restore\src-tauri\Plugins\Cache";

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

        Console.WriteLine($"Assembiles count: {assemblies.Length}");
        foreach (var assembly in assemblies)
        {
            Console.WriteLine($"Assembly name: {assembly.GetName().Name}");
        }

        services = services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IRepositoryPluginInitializer>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
        );
        IServiceProvider provider = services.BuildServiceProvider();
        IEnumerable<IRepositoryPluginInitializer> initializers =
            provider.GetServices<IRepositoryPluginInitializer>();
        Console.WriteLine($"Services count: {initializers.Count()}");
        foreach (IRepositoryPluginInitializer initializer in initializers)
        {
            Console.WriteLine($"Service: {initializer.GetType().Name}");
            initializer.Modify(services);
        }

        if (initializers.Count() == 0)
        {
            Console.WriteLine("No persistance services found");
        }
        else
        {
            Console.WriteLine("Persistance Plugins successfully installed");
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

        Console.WriteLine($"Assembiles count: {assemblies.Length}");
        foreach (var assembly in assemblies)
        {
            Console.WriteLine($"Assembly name: {assembly.GetName().Name}");
        }
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
        Console.WriteLine($"Services count: {initializers.Count()}");
        foreach (ICachedAdvertisementsPluginDiInitializer initializer in initializers)
        {
            Console.WriteLine($"Service: {initializer.GetType().Name}");
            initializer.AddCache(services);
        }

        if (initializers.Count() == 0)
        {
            Console.WriteLine("No instances of ICachedAdvertisementsPluginDiInitializer");
        }
        else
        {
            Console.WriteLine("Cache Plugins successfully installed");
        }
        return services;
    }
}
