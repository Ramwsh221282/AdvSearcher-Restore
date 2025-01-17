using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.FileSystem.SDK.Contracts;

public static class FileSystemPluginsLoader
{
    private static readonly string PluginsDirectory =
        $@"{Environment.CurrentDirectory}\Plugins\FileSystem";

    public static IServiceCollection LoadFileSystemPlugins(
        this IServiceCollection serviceCollection
    )
    {
        Assembly[] assemblies = Directory
            .GetFiles(PluginsDirectory, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();
        serviceCollection.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IFileSystemPluginLoader>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
        IServiceProvider provider = serviceCollection.BuildServiceProvider();
        IFileSystemPluginLoader loader = provider.GetRequiredService<IFileSystemPluginLoader>();
        loader.Load(serviceCollection);
        return serviceCollection;
    }
}
