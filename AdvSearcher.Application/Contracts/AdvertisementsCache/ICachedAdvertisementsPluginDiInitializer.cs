using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Application.Contracts.AdvertisementsCache;

public interface ICachedAdvertisementsPluginDiInitializer
{
    public IServiceCollection Modify(IServiceCollection services);
}
