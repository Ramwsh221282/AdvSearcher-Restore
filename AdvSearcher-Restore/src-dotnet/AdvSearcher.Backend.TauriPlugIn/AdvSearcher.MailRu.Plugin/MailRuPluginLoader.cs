using AdvSearcher.Publishing.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.MailRu.Plugin;

public class MailRuPluginLoader : IPublishingPluginsLoader
{
    public IServiceCollection Load(IServiceCollection services)
    {
        services = services.AddTransient<IMailingClient, MailRuService>();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Mail Ru Email client loaded.");
        return services;
    }
}
