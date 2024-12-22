using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Infrastracture.OkParser.Utils.Converters;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverCommands;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverCommands.ScrollToBottom;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverQueries;
using AdvSearcher.Infrastracture.OkParser.Utils.OkDriverQueries.ExtractHtml;
using AdvSearcher.Infrastracture.OkParser.Utils.OkHttpClients;
using AdvSearcher.Infrastracture.OkParser.Utils.OkWebDriver;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastracture.OkParser.DependencyInjection;

public static class OkParserServices
{
    public static IServiceCollection AddOkParser(this IServiceCollection services)
    {
        services = services
            .AddSingleton<IOkAdvertisementBuildersProvider, OkAdvertisementBuilderProvider>()
            .AddSingleton<IOkHttpClientProvider, OkHttpClientProvider>()
            .AddSingleton<IAdvertisementDateConverter<OkParser>, OkDateConverter>()
            .AddSingleton<IOkWebDriverDispatcher, OkWebDriverDispatcher>()
            .AddSingleton<OkWebDriverProvider>()
            .AddTransient<IOkDriverCommand<ScrollToBottomCommand>, ScrollToBottomCommand>()
            .AddTransient<IOkDriverQuery<ExtractHtmlQuery, string>, ExtractHtmlQuery>()
            .AddScoped<IParser<OkParserService>, OkParser>();
        return services;
    }
}
