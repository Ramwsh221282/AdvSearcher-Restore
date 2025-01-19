using AdvSearcher.Publishing.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.WhatsApp.Plugin.DependencyInjection;

public sealed class GreenApiPluginLoader : IPublishingPluginsLoader
{
    public IServiceCollection Load(IServiceCollection services)
    {
        services = services.AddTransient<IWhatsAppSender, GreenApiService>();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Green Api Publishing Service loaded.");
        return services;
    }
}
