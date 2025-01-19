using AdvSearcher.Persistance.SDK;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Persistance.SQLite;

public sealed class SQLitePersistanceDIService : IRepositoryPluginInitializer
{
    public IServiceCollection Modify(IServiceCollection services)
    {
        services = services.AddScoped<AppDbContext>();
        services = services
            .AddScoped<IAdvertisementsRepository, AdvertisementsRepository>(p =>
            {
                AppDbContext context = p.GetRequiredService<AppDbContext>();
                return new AdvertisementsRepository(context);
            })
            .AddScoped<IPublishersRepository, PublishersRepository>(p =>
            {
                AppDbContext context = p.GetRequiredService<AppDbContext>();
                return new PublishersRepository(context);
            })
            .AddScoped<IServiceUrlRepository, ServiceUrlsRepository>(p =>
            {
                AppDbContext context = p.GetRequiredService<AppDbContext>();
                return new ServiceUrlsRepository(context);
            });
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("SQLite Database service loaded");
        return services;
    }
}
