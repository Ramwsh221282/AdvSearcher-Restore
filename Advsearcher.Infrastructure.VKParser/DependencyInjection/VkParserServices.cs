using Advsearcher.Infrastructure.VKParser.Components.Converters;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using Advsearcher.Infrastructure.VKParser.Components.VkParserChain;
using Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using Microsoft.Extensions.DependencyInjection;

namespace Advsearcher.Infrastructure.VKParser.DependencyInjection;

public static class VkParserServices
{
    public static IServiceCollection AddVkParser(this IServiceCollection services)
    {
        services = services
            .AddTransient<VkDateConverter>()
            .AddTransient<VkParserPipeLine>()
            .AddTransient<IParser<VkParserService>, VkParser>()
            .AddTransient<IVkOptionsProvider, TestVkOptionsProvider>()
            .AddTransient<IVkParserRequestFactory, VkParserRequestFactory>()
            .AddTransient<IVkParserNode>(p =>
            {
                VkParserPipeLine pipeLine = p.GetRequiredService<VkParserPipeLine>();
                IHttpService httpService = p.GetRequiredService<IHttpService>();
                IHttpClient httpClient = p.GetRequiredService<IHttpClient>();
                IVkParserRequestFactory requestFactory =
                    p.GetRequiredService<IVkParserRequestFactory>();

                IVkParserNode responseNode = new CreateVkParserResponseNode(
                    pipeLine,
                    httpService,
                    httpClient,
                    requestFactory
                );

                IVkParserNode itemJsonNode = new CreateVkItemsJsonNode(
                    pipeLine,
                    httpClient,
                    httpService,
                    requestFactory,
                    responseNode
                );

                IVkParserNode groupInfoNode = new CreateVkGroupInfoNode(
                    pipeLine,
                    httpService,
                    httpClient,
                    itemJsonNode
                );

                IVkParserNode parametersNode = new CreateRequestParametersNode(
                    pipeLine,
                    groupInfoNode
                );

                return parametersNode;
            });
        return services;
    }
}
