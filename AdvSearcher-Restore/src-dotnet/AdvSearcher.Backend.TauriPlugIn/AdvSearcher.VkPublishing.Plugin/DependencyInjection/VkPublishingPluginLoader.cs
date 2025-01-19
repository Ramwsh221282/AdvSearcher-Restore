using AdvSearcher.Publishing.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.VkPublishing.Plugin.DependencyInjection;

public class VkPublishingPluginLoader : IPublishingPluginsLoader
{
    public IServiceCollection Load(IServiceCollection services)
    {
        services.AddTransient<IPublishingService, VkPublishingService>();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Vk publishing service loaded.");
        return services;
    }
}
