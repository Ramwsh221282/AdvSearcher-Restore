using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.MachineLearning.SDK;

public static class MLDependencyInjection
{
    private static readonly string PluginsPath = $@"{Environment.CurrentDirectory}\Plugins\ML";

    public static IServiceCollection LoadML(this IServiceCollection services)
    {
        Console.WriteLine("Loading ML Plugins");
        Assembly[] assemblies = Directory
            .GetFiles(PluginsPath, "*.dll")
            .Select(Assembly.LoadFrom)
            .ToArray();
        Console.WriteLine($"ML Assemblies count: {assemblies.Length}");
        services = services.Scan(x =>
            x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IMLPluginLoader>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
        IServiceProvider provider = services.BuildServiceProvider();
        IEnumerable<IMLPluginLoader> loaders = provider.GetServices<IMLPluginLoader>();
        Console.WriteLine($"Plugins Count: {loaders.Count()}");
        foreach (IMLPluginLoader loader in loaders)
        {
            Console.WriteLine(loader.GetType().Name);
            loader.LoadML(services);
        }
        return services;
    }
}
