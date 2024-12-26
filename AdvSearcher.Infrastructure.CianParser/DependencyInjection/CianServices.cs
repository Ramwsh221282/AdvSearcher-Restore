using AdvSearcher.Infrastructure.CianParser.CianParserChains;
using AdvSearcher.Infrastructure.CianParser.CianParserChains.Nodes;
using AdvSearcher.Infrastructure.CianParser.Utils.Converters;
using AdvSearcher.Parser.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.CianParser.DependencyInjection;

public static class CianServices
{
    public static IServiceCollection AddCianParser(this IServiceCollection services)
    {
        services = services
            .AddTransient<CianDateConverter>()
            .AddTransient<CianParserPipeLine>()
            .AddScoped<IParser<CianParserService>, CianParser>()
            .AddTransient<ICianParserChain>(p =>
            {
                CianParserPipeLine pipeLine = p.GetRequiredService<CianParserPipeLine>();
                ICianParserChain constructNode = new ConstructResponseNode(pipeLine);
                ICianParserChain initializeCardsNode = new InitializeCianAdvertisementCardsNode(
                    pipeLine,
                    constructNode
                );
                ICianParserChain cardElementsNode = new SetCardElementsNode(
                    pipeLine,
                    initializeCardsNode
                );
                ICianParserChain wrapperNode = new SetCardsWrapperElementNode(
                    pipeLine,
                    cardElementsNode
                );
                ICianParserChain openPage = new OpenCianCataloguePageNode(pipeLine, wrapperNode);
                return openPage;
            });
        return services;
    }
}
