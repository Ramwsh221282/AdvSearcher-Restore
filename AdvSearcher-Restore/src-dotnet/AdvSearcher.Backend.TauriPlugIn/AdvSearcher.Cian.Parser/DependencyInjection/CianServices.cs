using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Cian.Parser.CianParserChains;
using AdvSearcher.Cian.Parser.CianParserChains.Nodes;
using AdvSearcher.Cian.Parser.Utils.Converters;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Cian.Parser.DependencyInjection;

public class CianServices : IParserDiServicesInitializer
{
    private IMessageListener _listener;

    public CianServices(IMessageListener listener)
    {
        _listener = listener;
        SetMessageListener(_listener);
    }

    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddScoped<CianDateConverter>()
            .AddScoped<CianParserPipeLine>()
            .AddScoped<IParser, CianParser>(p =>
            {
                ICianParserChain chain = p.GetRequiredService<ICianParserChain>();
                CianParser parser = new CianParser(chain);
                parser.SetMessageListener(_listener);
                return parser;
            })
            .AddScoped<ICianParserChain>(p =>
            {
                CianParserPipeLine pipeLine = p.GetRequiredService<CianParserPipeLine>();
                ICianParserChain constructNode = new ConstructResponseNode(pipeLine, _listener);
                ICianParserChain initializeCardsNode = new InitializeCianAdvertisementCardsNode(
                    pipeLine,
                    _listener,
                    constructNode
                );
                ICianParserChain cardElementsNode = new SetCardElementsNode(
                    pipeLine,
                    initializeCardsNode
                );
                ICianParserChain wrapperNode = new SetCardsWrapperElementNode(
                    pipeLine,
                    _listener,
                    cardElementsNode
                );
                ICianParserChain openPage = new OpenCianCataloguePageNode(
                    pipeLine,
                    _listener,
                    wrapperNode
                );
                return openPage;
            });
        _listener.Publish("Парсер Циан подгружен");
        return services;
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
