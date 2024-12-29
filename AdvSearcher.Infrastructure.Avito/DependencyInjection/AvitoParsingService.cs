using AdvSearcher.Infrastructure.Avito.AvitoParserChain;
using AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;
using AdvSearcher.Infrastructure.Avito.Utils.Converters;
using AdvSearcher.Parser.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.Avito.DependencyInjection;

public static class AvitoParsingService
{
    public static IServiceCollection AddAvitoParsingServices(this IServiceCollection services)
    {
        services = services
            .AddTransient<IParser, AvitoParser>()
            .AddTransient<AvitoDateConverter>()
            .AddTransient<AvitoParserPipeLine>()
            .AddTransient<IAvitoChainNode>(p =>
            {
                AvitoParserPipeLine pipeLine = p.GetRequiredService<AvitoParserPipeLine>();
                IAvitoChainNode factoryNode = new ConstructAvitoResponseNode(pipeLine);
                IAvitoChainNode itemsNode = new InitializeAvitoCatalogueItemPhoneAndPhotosNode(
                    pipeLine,
                    factoryNode
                );
                IAvitoChainNode catalogueNode = new ExtractAvitoCatalogueNode(pipeLine, itemsNode);
                return catalogueNode;
            });
        return services;
    }
}
