using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDriverCommands;
using AdvSearcher.Infrastructure.CianParser.Utils.CianWebDrivers;
using AdvSearcher.Infrastructure.CianParser.Utils.Factories;

namespace AdvSearcher.Infrastructure.CianParser;

internal sealed class CianParser(
    CianWebDriverProvider driverProvider,
    ICianWebDriverCommandDispatcher dispatcher,
    IAdvertisementDateConverter<CianParser> converter
) : IParser<CianParserService>
{
    private readonly List<IParserResponse> _results = [];

    public async Task<Result<bool>> ParseData(ServiceUrl url)
    {
        var cards = await GetCardsFromWebDriverScraping(url);
        var advertisements = GetConstructedResponseFromFactory(cards);
        if (advertisements.Count == 0)
            return ParserErrors.NoAdvertisementsParsed;
        _results.AddRange(advertisements);
        return true;
    }

    private async Task<List<CianAdvertisementCard>> GetCardsFromWebDriverScraping(ServiceUrl url)
    {
        using var driver = driverProvider.BuildWebDriver();
        var method = new CianParsingMethod(url);
        return await method.BuildAdvertisementCards(driver, dispatcher);
    }

    private IReadOnlyCollection<IParserResponse> GetConstructedResponseFromFactory(
        List<CianAdvertisementCard> cards
    )
    {
        var factory = new CianAdvertisementsFactory(cards, converter);
        factory.Construct();
        return factory.Results;
    }

    public IReadOnlyCollection<IParserResponse> Results => _results;
}
