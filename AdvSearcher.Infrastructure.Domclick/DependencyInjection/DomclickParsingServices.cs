using AdvSearcher.Infrastructure.Domclick.DomclickParserChains;
using AdvSearcher.Infrastructure.Domclick.DomclickParserChains.Nodes;
using AdvSearcher.Infrastructure.Domclick.HttpRequests;
using AdvSearcher.Infrastructure.Domclick.InternalModels;
using AdvSearcher.Parser.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastructure.Domclick.DependencyInjection;

public static class DomclickParsingServices
{
    public static IServiceCollection AddDomclickParsingServices(this IServiceCollection services)
    {
        services = services
            .AddTransient<IParser<DomclickParserService>, DomclickParser>()
            .AddTransient<IDomclickFetchingResultFactory, DomclickFetchingResultFactory>()
            .AddTransient<DomclickParserPipeline>()
            .AddTransient<DomclickRequestHandler>()
            .AddTransient<IDomclickParserChain>(p =>
            {
                DomclickParserPipeline pipeline = p.GetRequiredService<DomclickParserPipeline>();
                DomclickRequestHandler handler = p.GetRequiredService<DomclickRequestHandler>();
                IDomclickFetchingResultFactory factory =
                    p.GetRequiredService<IDomclickFetchingResultFactory>();
                IDomclickParserChain disposeNode = new DomclickDependencyDisposeNode(
                    pipeline,
                    handler
                );
                IDomclickParserChain responseNode = new DomclickInitializeResponseNode(
                    pipeline,
                    disposeNode
                );
                IDomclickParserChain phoneNode = new DomclickInitializePhoneNumberNode(
                    pipeline,
                    handler,
                    responseNode
                );
                IDomclickParserChain cookieNode = new DomclickCookieInitializerNode(
                    pipeline,
                    phoneNode
                );
                IDomclickParserChain httpFetchNode = new DomClickPageHttpFetchNode(
                    pipeline,
                    handler,
                    factory,
                    cookieNode
                );
                IDomclickParserChain qratorNode = new DomclickInitializeQratorTokensNode(
                    pipeline,
                    httpFetchNode
                );
                IDomclickParserChain fakeProcess = new DomClickStartFakeProcessNode(
                    pipeline,
                    qratorNode
                );
                return fakeProcess;
            });
        return services;
    }
}
