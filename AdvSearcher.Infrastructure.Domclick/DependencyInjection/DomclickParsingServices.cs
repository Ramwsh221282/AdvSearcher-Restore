using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;
using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.Domclick.DependencyInjection;

public class DomclickParsingServices : IParserDIServicesInitializer
{
    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddTransient<IParser, DomclickParser>()
            .AddTransient<IDomclickFetchingResultFactory, DomclickFetchingResultFactory>()
            .AddTransient<DomclickParserPipeline>()
            .AddTransient<DomclickRequestHandler>()
            .AddTransient<IDomclickParserChain>(p =>
            {
                DomclickParserPipeline pipeline = p.GetRequiredService<DomclickParserPipeline>();
                ParserConsoleLogger logger = p.GetRequiredService<ParserConsoleLogger>();
                IDomclickFetchingResultFactory factory =
                    p.GetRequiredService<IDomclickFetchingResultFactory>();
                WebDriverProvider provider = p.GetRequiredService<WebDriverProvider>();
                IDomclickParserChain thirdStep = new DomclickResultsInitializerStep(
                    pipeline,
                    logger
                );
                IDomclickParserChain secondStep = new DomclickInitializeMobilePhoneStep(
                    pipeline,
                    logger,
                    thirdStep
                );
                IDomclickParserChain firstStep = new DomclickCatalogueFetchStep(
                    pipeline,
                    factory,
                    provider,
                    logger,
                    secondStep
                );
                return firstStep;
            });
        return services;
    }
}
