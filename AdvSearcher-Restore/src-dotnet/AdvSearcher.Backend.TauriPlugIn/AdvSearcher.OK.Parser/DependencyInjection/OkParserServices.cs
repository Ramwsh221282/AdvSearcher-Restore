using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.OK.Parser.OkParserChains;
using AdvSearcher.OK.Parser.OkParserChains.Nodes;
using AdvSearcher.OK.Parser.Utils.Converters;
using AdvSearcher.OK.Parser.Utils.Factory.Builders;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.OK.Parser.DependencyInjection;

public class OkParserServices : IParserDiServicesInitializer
{
    private IMessageListener _listener;

    public OkParserServices(IMessageListener listener)
    {
        _listener = listener;
        SetMessageListener(_listener);
    }

    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddScoped<OkDateConverter>()
            .AddScoped<OkParserPipeLine>()
            .AddScoped<IOkAdvertisementBuildersProvider, OkAdvertisementBuilderProvider>()
            .AddScoped<IParser, OkParser>(p =>
            {
                IOkParserChain chain = p.GetRequiredService<IOkParserChain>();
                OkParser parser = new OkParser(chain);
                parser.SetMessageListener(_listener);
                return parser;
            })
            .AddScoped<IOkParserChain>(p =>
            {
                OkParserPipeLine pipeLine = p.GetRequiredService<OkParserPipeLine>();
                WebDriverProvider provider = p.GetRequiredService<WebDriverProvider>();
                IHttpClient httpClient = p.GetRequiredService<IHttpClient>();
                IOkParserChain buildResponseChain = new CreateAdvertisementResponseNode(
                    pipeLine,
                    httpClient,
                    _listener
                );
                IOkParserChain extractHtmlNode = new ExtractHtmlNode(
                    pipeLine,
                    provider,
                    _listener,
                    buildResponseChain
                );
                return extractHtmlNode;
            });
        _listener.Publish("Парсер ОК подгружен");
        return services;
    }

    public void SetMessageListener(IMessageListener listener) => _listener = listener;
}
