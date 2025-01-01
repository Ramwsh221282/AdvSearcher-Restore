using AdvSearcher.Persistance.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SQLite;

public sealed class SQLitePersistanceDIService : IRepositoryPluginInitializer
{
    public IServiceCollection Modify(IServiceCollection services)
    {
        services = services
            .AddScoped<IAdvertisementsRepository, AdvertisementsRepository>()
            .AddScoped<IPublishersRepository, PublishersRepository>()
            .AddScoped<IServiceUrlRepository, ServiceUrlsRepository>();
        return services;
    }
}
