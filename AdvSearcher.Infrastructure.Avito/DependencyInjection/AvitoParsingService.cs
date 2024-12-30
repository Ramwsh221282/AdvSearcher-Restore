using AdvSearcher.Infrastructure.Avito.AvitoParserChain;
using AdvSearcher.Infrastructure.Avito.AvitoParserChain.Nodes;
using AdvSearcher.Infrastructure.Avito.Utils.Converters;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.Avito.DependencyInjection;

public class AvitoParsingService : IParserDIServicesInitializer
{
    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddTransient<IParser, AvitoParser>()
            .AddTransient<AvitoDateConverter>()
            .AddTransient<AvitoParserPipeLine>()
            .AddTransient<IAvitoChainNode>(p =>
            {
                AvitoParserPipeLine pipeLine = p.GetRequiredService<AvitoParserPipeLine>();
                ParserConsoleLogger logger = p.GetRequiredService<ParserConsoleLogger>();
                IAvitoChainNode factoryNode = new ConstructAvitoResponseNode(pipeLine, logger);
                IAvitoChainNode itemsNode = new InitializeAvitoCatalogueItemPhoneAndPhotosNode(
                    pipeLine,
                    logger,
                    factoryNode
                );
                IAvitoChainNode catalogueNode = new ExtractAvitoCatalogueNode(
                    pipeLine,
                    logger,
                    itemsNode
                );
                return catalogueNode;
            });
        return services;
    }
}
