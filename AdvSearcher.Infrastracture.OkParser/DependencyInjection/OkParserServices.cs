using AdvSearcher.Infrastracture.OkParser.OkParserChains;
using AdvSearcher.Infrastracture.OkParser.OkParserChains.Nodes;
using AdvSearcher.Infrastracture.OkParser.Utils.Converters;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastracture.OkParser.DependencyInjection;

public class OkParserServices : IParserDIServicesInitializer
{
    public IServiceCollection ModifyServices(IServiceCollection services)
    {
        services = services
            .AddTransient<OkDateConverter>()
            .AddTransient<OkParserPipeLine>()
            .AddTransient<IOkAdvertisementBuildersProvider, OkAdvertisementBuilderProvider>()
            .AddTransient<IParser, OkParser>()
            .AddTransient<IOkParserChain>(p =>
            {
                OkParserPipeLine pipeLine = p.GetRequiredService<OkParserPipeLine>();
                WebDriverProvider provider = p.GetRequiredService<WebDriverProvider>();
                IHttpClient httpClient = p.GetRequiredService<IHttpClient>();
                ParserConsoleLogger logger = p.GetRequiredService<ParserConsoleLogger>();

                IOkParserChain buildResponseChain = new CreateAdvertisementResponseNode(
                    pipeLine,
                    httpClient,
                    logger
                );

                IOkParserChain extractHtmlNode = new ExtractHtmlNode(
                    pipeLine,
                    provider,
                    logger,
                    buildResponseChain
                );
                return extractHtmlNode;
            });
        return services;
    }
}
