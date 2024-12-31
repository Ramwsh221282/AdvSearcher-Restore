using Advsearcher.Infrastructure.VKParser.Components.Converters;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.VkParserChain;
using Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using Microsoft.Extensions.DependencyInjection;

namespace Advsearcher.Infrastructure.VKParser.DependencyInjection;

public class VkParserServices : IParserDIServicesInitializer
{
    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddTransient<VkDateConverter>()
            .AddTransient<VkParserPipeLine>()
            .AddTransient<IParser, VkParser>()
            .AddTransient<IVkOptionsProvider, VkOptionsProvider>()
            .AddTransient<IVkParserRequestFactory, VkParserRequestFactory>()
            .AddTransient<IVkParserNode>(p =>
            {
                VkParserPipeLine pipeLine = p.GetRequiredService<VkParserPipeLine>();
                IHttpService httpService = p.GetRequiredService<IHttpService>();
                IHttpClient httpClient = p.GetRequiredService<IHttpClient>();
                IVkParserRequestFactory requestFactory =
                    p.GetRequiredService<IVkParserRequestFactory>();
                ParserConsoleLogger logger = p.GetRequiredService<ParserConsoleLogger>();

                IVkParserNode responseNode = new CreateVkParserResponseNode(
                    pipeLine,
                    httpService,
                    httpClient,
                    requestFactory,
                    logger
                );

                IVkParserNode itemJsonNode = new CreateVkItemsJsonNode(
                    pipeLine,
                    httpClient,
                    httpService,
                    requestFactory,
                    logger,
                    responseNode
                );

                IVkParserNode groupInfoNode = new CreateVkGroupInfoNode(
                    pipeLine,
                    httpService,
                    httpClient,
                    logger,
                    itemJsonNode
                );

                IVkParserNode parametersNode = new CreateRequestParametersNode(
                    pipeLine,
                    logger,
                    groupInfoNode
                );

                return parametersNode;
            });
        return services;
    }
}
