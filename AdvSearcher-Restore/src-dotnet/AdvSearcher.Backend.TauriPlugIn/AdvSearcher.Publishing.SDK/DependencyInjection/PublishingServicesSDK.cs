using System.Reflection;
using AdvSearcher.Publishing.SDK.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Publishing.SDK.DependencyInjection;

public static class PublishingServicesSDK
{
    private static readonly string PluginsPath =
        $@"{Environment.CurrentDirectory}\Plugins\Publishing";

    public static IServiceCollection AddPublishingServices(this IServiceCollection services)
    {
        Assembly[] assemblies = Directory
            .GetFiles(PluginsPath, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();

        services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IPublishingPluginsLoader>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );

        IServiceProvider provider = services.BuildServiceProvider();
        IEnumerable<IPublishingPluginsLoader> loaders =
            provider.GetServices<IPublishingPluginsLoader>();
        foreach (var loader in loaders)
            loader.Load(services);

        services.AddSingleton<PublishingLogger>();
        services.AddTransient<PublishingServiceProvider>();

        return services;
    }
}
