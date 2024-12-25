using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Infrastracture.OkParser.Utils.Converters;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastracture.OkParser.DependencyInjection;

public static class OkParserServices
{
    public static IServiceCollection AddOkParser(this IServiceCollection services)
    {
        services = services
            .AddScoped<IOkAdvertisementBuildersProvider, OkAdvertisementBuilderProvider>()
            .AddScoped<IAdvertisementDateConverter<OkParser>, OkDateConverter>()
            .AddScoped<IParser<OkParserService>, OkParser>()
            .AddScoped<OkAdvertisementsFactory>()
            .AddScoped<OkHtmlExtractor>();
        return services;
    }
}
