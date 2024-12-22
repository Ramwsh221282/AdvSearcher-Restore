using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Infrastructure.Avito.Utils.Converters;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.ScrollToBottom;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverQueries.ExtractHtml;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.Avito.DependencyInjection;

public static class AvitoParsingService
{
    public static IServiceCollection AddAvitoParsingServices(this IServiceCollection services)
    {
        services = services
            .AddSingleton<AvitoWebDriverProvider>()
            .AddScoped<IAvitoWebDriverDispatcher, AvitoWebDriverDispatcher>()
            .AddTransient<IAvitoWebDriverCommand<ScrollToBottomCommand>, ScrollToBottomCommand>()
            .AddTransient<IAvitoWebDriverQuery<ExtractHtmlQuery, string>, ExtractHtmlQuery>()
            .AddScoped<IParser<AvitoParserService>, AvitoParser>()
            .AddSingleton<IAdvertisementDateConverter<AvitoParser>, AvitoDateConverter>();
        return services;
    }
}
