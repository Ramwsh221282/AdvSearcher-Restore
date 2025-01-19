using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.MachineLearning.SDK;

public static class MLDependencyInjection
{
    private static readonly string PluginsPath = $@"{Environment.CurrentDirectory}\Plugins\ML";

    public static IServiceCollection LoadML(this IServiceCollection services)
    {
        Assembly[] assemblies = Directory
            .GetFiles(PluginsPath, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();
        services = services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IMLPluginLoader>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
        IServiceProvider provider = services.BuildServiceProvider();
        IEnumerable<IMLPluginLoader> loaders = provider.GetServices<IMLPluginLoader>();
        if (!loaders.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            throw new ApplicationException();
        }
        foreach (IMLPluginLoader loader in loaders)
            loader.LoadML(services);
        return services;
    }
}
