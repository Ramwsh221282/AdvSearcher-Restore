using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Utils.WebDrivers;

namespace AdvSearcher.Infrastructure.Avito;

internal sealed class AvitoParser(
    AvitoWebDriverProvider provider,
    IAvitoWebDriverDispatcher driverDispatcher,
    IAdvertisementDateConverter<AvitoParserService> converter
) : IParser<AvitoParserService>
{
    private readonly List<IParserResponse> _results = [];

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        using var driver = provider.BuildWebDriver();
        var method = new AvitoParsingMethod(driverDispatcher);
        var items = await method.ExecuteMethod(driver, url);
        var factory = new AvitoAdvertisementsFactory(driver);
        await factory.Construct(converter, items.Value);
        _results.AddRange(factory.Results);
        return true;
    }

    public IReadOnlyCollection<IParserResponse> Results => _results;
}
