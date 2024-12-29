using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;
using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.Domclick.DependencyInjection;

public static class DomclickParsingServices
{
    public static IServiceCollection AddDomclickParsingServices(this IServiceCollection services)
    {
        services = services
            .AddTransient<IParser, DomclickParser>()
            .AddTransient<IDomclickFetchingResultFactory, DomclickFetchingResultFactory>()
            .AddTransient<DomclickParserPipeline>()
            .AddTransient<DomclickRequestHandler>()
            .AddTransient<IDomclickParserChain>(p =>
            {
                DomclickParserPipeline pipeline = p.GetRequiredService<DomclickParserPipeline>();
                IDomclickFetchingResultFactory factory =
                    p.GetRequiredService<IDomclickFetchingResultFactory>();
                WebDriverProvider provider = p.GetRequiredService<WebDriverProvider>();
                IDomclickParserChain thirdStep = new DomclickResultsInitializerStep(pipeline);
                IDomclickParserChain secondStep = new DomclickInitializeMobilePhoneStep(
                    pipeline,
                    thirdStep
                );
                IDomclickParserChain firstStep = new DomclickCatalogueFetchStep(
                    pipeline,
                    factory,
                    provider,
                    secondStep
                );
                return firstStep;
            });
        return services;
    }
}
