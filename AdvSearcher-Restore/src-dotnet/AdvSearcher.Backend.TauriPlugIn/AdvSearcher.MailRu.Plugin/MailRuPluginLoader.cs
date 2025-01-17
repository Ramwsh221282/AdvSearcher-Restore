using AdvSearcher.Publishing.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.MailRu.Plugin;

public class MailRuPluginLoader : IPublishingPluginsLoader
{
    public IServiceCollection Load(IServiceCollection services)
    {
        services = services.AddTransient<IMailingClient, MailRuService>();
        return services;
    }
}
