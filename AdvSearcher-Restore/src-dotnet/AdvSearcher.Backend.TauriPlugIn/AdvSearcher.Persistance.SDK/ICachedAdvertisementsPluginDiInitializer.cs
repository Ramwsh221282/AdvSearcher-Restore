using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SDK;

public interface ICachedAdvertisementsPluginDiInitializer
{
    public IServiceCollection AddCache(IServiceCollection services);
}
