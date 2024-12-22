using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using Advsearcher.Infrastructure.VKParser.Components.Converters;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Models.HttpSenders;
using Microsoft.Extensions.DependencyInjection;

namespace Advsearcher.Infrastructure.VKParser.DependencyInjection;

public static class VkParserServices
{
    public static IServiceCollection AddVkParser(this IServiceCollection services)
    {
        services = services
            .AddScoped<IParser<VkParserService>, VkParser>()
            .AddSingleton<IVkHttpSender, VkHttpSender>()
            .AddSingleton<IVkOptionsProvider, TestVkOptionsProvider>()
            .AddSingleton<IAdvertisementDateConverter<VkParser>, VkDateConverter>()
            .AddScoped<IVkParserRequestFactory, VkParserRequestFactory>();
        return services;
    }
}
