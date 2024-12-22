using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Entities.ServiceUrls.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;
using AdvSearcher.Infrastracture.OkParser.Utils.Materials;
using AdvSearcher.Infrastracture.OkParser.Utils.OkHttpClients;
using AdvSearcher.Infrastracture.OkParser.Utils.OkWebDriver;

namespace AdvSearcher.Infrastracture.OkParser;

internal sealed class OkParser(
    OkWebDriverProvider provider,
    IOkWebDriverDispatcher dispatcher,
    IOkAdvertisementBuildersProvider builders,
    IAdvertisementDateConverter<OkParser> converter,
    IOkHttpClientProvider httpProvider
) : IParser<OkParserService>
{
    private readonly List<IParserResponse> _results = [];

    public IReadOnlyCollection<IParserResponse> Results => _results;

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        if (url.Mode != ServiceUrlMode.Loadable)
            return ParserErrors.UrlIsNotForLoading;

        using var driver = provider.BuildWebDriver();
        var method = new OkParsingMethod(driver, dispatcher);

        var html = await method.ExtractPageHtml(url);
        if (string.IsNullOrWhiteSpace(html))
            return ParserErrors.HtmlEmpty;

        var nodes = new OkTopicNodes(html);

        var factory = new OkAdvertisementsFactory(url, nodes);
        await factory.Construct(builders, converter, httpProvider);
        _results.AddRange(factory.Results);
        return true;
    }
}
