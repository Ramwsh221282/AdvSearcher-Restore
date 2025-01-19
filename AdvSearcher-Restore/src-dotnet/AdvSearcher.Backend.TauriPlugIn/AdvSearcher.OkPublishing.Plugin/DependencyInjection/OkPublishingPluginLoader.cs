using AdvSearcher.Publishing.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.OkPublishing.Plugin.DependencyInjection;

public sealed class OkPublishingPluginLoader : IPublishingPluginsLoader
{
    public IServiceCollection Load(IServiceCollection services)
    {
        services = services.AddTransient<IPublishingService, OkPublishingService>();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Ok Publishing service loaded.");
        return services;
    }
}
