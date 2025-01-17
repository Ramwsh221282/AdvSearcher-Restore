using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Domclick.Parser.DomclickParserChains;
using AdvSearcher.Domclick.Parser.DomclickParserChains.Nodes;
using AdvSearcher.Domclick.Parser.HttpRequests;
using AdvSearcher.Domclick.Parser.InternalModels;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Domclick.Parser.DependencyInjection;

public class DomclickParsingServices : IParserDiServicesInitializer
{
    private IMessageListener _listener;

    public DomclickParsingServices(IMessageListener listener)
    {
        _listener = listener;
        SetMessageListener(_listener);
    }

    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddScoped<IParser, DomclickParser>(p =>
            {
                IDomclickParserChain chain = p.GetRequiredService<IDomclickParserChain>();
                DomclickParser parser = new DomclickParser(chain);
                parser.SetMessageListener(_listener);
                return parser;
            })
            .AddScoped<IDomclickFetchingResultFactory, DomclickFetchingResultFactory>()
            .AddScoped<DomclickParserPipeline>()
            .AddScoped<DomclickRequestHandler>()
            .AddScoped<IDomclickParserChain>(p =>
            {
                DomclickParserPipeline pipeline = p.GetRequiredService<DomclickParserPipeline>();
                IDomclickFetchingResultFactory factory =
                    p.GetRequiredService<IDomclickFetchingResultFactory>();
                WebDriverProvider provider = p.GetRequiredService<WebDriverProvider>();
                IDomclickParserChain thirdStep = new DomclickResultsInitializerStep(
                    pipeline,
                    _listener
                );
                IDomclickParserChain secondStep = new DomclickInitializeMobilePhoneStep(
                    pipeline,
                    _listener,
                    thirdStep
                );
                IDomclickParserChain firstStep = new DomclickCatalogueFetchStep(
                    pipeline,
                    factory,
                    provider,
                    _listener,
                    secondStep
                );
                return firstStep;
            });
        _listener.Publish("Парсер ДомКлик подгружен");
        return services;
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
