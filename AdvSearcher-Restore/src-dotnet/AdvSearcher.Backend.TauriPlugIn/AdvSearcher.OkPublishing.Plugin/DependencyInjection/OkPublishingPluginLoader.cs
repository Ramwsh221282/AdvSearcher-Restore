using AdvSearcher.Publishing.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.OkPublishing.Plugin.DependencyInjection;

public sealed class OkPublishingPluginLoader : IPublishingPluginsLoader
{
    public IServiceCollection Load(IServiceCollection services)
    {
        services = services.AddTransient<IPublishingService, OkPublishingService>();
        return services;
    }
}
