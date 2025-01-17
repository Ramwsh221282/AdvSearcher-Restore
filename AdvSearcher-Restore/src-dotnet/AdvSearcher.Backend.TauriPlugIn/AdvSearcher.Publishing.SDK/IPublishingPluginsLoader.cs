using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Publishing.SDK;

public interface IPublishingPluginsLoader
{
    IServiceCollection Load(IServiceCollection services);
}
