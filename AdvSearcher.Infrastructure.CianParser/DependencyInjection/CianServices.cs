using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands.ScrollToBottom;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands.ScrollToTop;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDrivers;
using AdvSearcher.Infrastructure.CianParser.Utils.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.CianParser.DependencyInjection;

public static class CianServices
{
    public static IServiceCollection AddCianParser(this IServiceCollection services)
    {
        services = services
            .AddSingleton<CianWebDriverProvider>()
            .AddSingleton<ICianWebDriverCommandDispatcher, CianWebDriverCommandDispatcher>()
            .AddSingleton<IAdvertisementDateConverter<CianParser>, CianDateConverter>()
            .AddScoped<IParser<CianParserService>, CianParser>()
            .AddTransient<ICianWebDriverCommand<ScrollToBottomCommand>, ScrollToBottomCommand>()
            .AddTransient<ICianWebDriverCommand<ScrollToTopCommand>, ScrollToTopCommand>();
        return services;
    }
}
