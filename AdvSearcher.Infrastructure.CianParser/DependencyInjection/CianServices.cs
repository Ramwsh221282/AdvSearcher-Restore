using AdvSearcher.Infrastructure.CianParser.CianParserChains;
using AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;
using AdvSearcher.Infrastructure.CianParser.Utils.Converters;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.CianParser.DependencyInjection;

public class CianServices : IParserDIServicesInitializer
{
    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddTransient<CianDateConverter>()
            .AddTransient<CianParserPipeLine>()
            .AddScoped<IParser, CianParser>()
            .AddTransient<ICianParserChain>(p =>
            {
                CianParserPipeLine pipeLine = p.GetRequiredService<CianParserPipeLine>();
                ParserConsoleLogger logger = p.GetRequiredService<ParserConsoleLogger>();
                ICianParserChain constructNode = new ConstructResponseNode(pipeLine, logger);
                ICianParserChain initializeCardsNode = new InitializeCianAdvertisementCardsNode(
                    pipeLine,
                    logger,
                    constructNode
                );
                ICianParserChain cardElementsNode = new SetCardElementsNode(
                    pipeLine,
                    logger,
                    initializeCardsNode
                );
                ICianParserChain wrapperNode = new SetCardsWrapperElementNode(
                    pipeLine,
                    logger,
                    cardElementsNode
                );
                ICianParserChain openPage = new OpenCianCataloguePageNode(
                    pipeLine,
                    logger,
                    wrapperNode
                );
                return openPage;
            });
        return services;
    }
}
