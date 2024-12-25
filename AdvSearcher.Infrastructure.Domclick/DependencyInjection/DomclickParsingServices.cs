using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Infrastructure.Domclick.DomclickWebDriver;
using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.Domclick.DependencyInjection;

public static class DomclickParsingServices
{
    public static IServiceCollection AddDomclickParsingServices(this IServiceCollection services)
    {
        services = services
            .AddScoped<IParser<DomclickParserService>, DomclickParser>()
            .AddScoped<IDomclickRequestExecutor, DomclickHttpSender>()
            .AddScoped<IDomclickFetchingResultFactory, DomclickFetchingResultFactory>()
            .AddScoped<WebDriverProvider>()
            .AddScoped<DomclickAdvertisementsFactory>();
        return services;
    }
}
