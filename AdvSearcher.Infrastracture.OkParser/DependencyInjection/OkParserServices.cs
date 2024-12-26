using AdvSearcher.Infrastracture.OkParser.OkParserChains;
using AdvSearcher.Infrastracture.OkParser.OkParserChains.Nodes;
using AdvSearcher.Infrastracture.OkParser.Utils.Converters;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.Parser.SDK.WebDriverParsing;
using Microsoft.Extensions.DependencyInjection;

namespace AdvSearcher.Infrastracture.OkParser.DependencyInjection;

public static class OkParserServices
{
    public static IServiceCollection AddOkParser(this IServiceCollection services)
    {
        services = services
            .AddTransient<OkDateConverter>()
            .AddTransient<OkParserPipeLine>()
            .AddTransient<IOkAdvertisementBuildersProvider, OkAdvertisementBuilderProvider>()
            .AddTransient<IParser<OkParserService>, OkParser>()
            .AddTransient<IOkParserChain>(p =>
            {
                OkParserPipeLine pipeLine = p.GetRequiredService<OkParserPipeLine>();
                WebDriverProvider provider = p.GetRequiredService<WebDriverProvider>();
                IHttpClient httpClient = p.GetRequiredService<IHttpClient>();

                IOkParserChain buildResponseChain = new CreateAdvertisementResponseNode(
                    pipeLine,
                    httpClient
                );

                IOkParserChain extractHtmlNode = new ExtractHtmlNode(
                    pipeLine,
                    provider,
                    buildResponseChain
                );
                return extractHtmlNode;
            });
        return services;
    }
}
