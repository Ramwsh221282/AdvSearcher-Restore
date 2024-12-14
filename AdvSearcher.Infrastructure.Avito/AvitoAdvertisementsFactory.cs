using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito;

internal sealed class AvitoAdvertisementsFactory(IWebDriver driver)
{
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public async Task Construct(
        IAdvertisementDateConverter<AvitoParser> converter,
        List<AvitoCatalogueItem> items
    )
    {
        foreach (var item in items)
        {
            await ExecuteOpenPageAction(item);
            var fetchGalleryCommand = new FetchGalleryItemsCommand(item);
            await fetchGalleryCommand.Execute(driver);
            var fetchMobilePhoneCommand = new FetchMobilePhone(item);
            await fetchMobilePhoneCommand.Execute(driver);
        }
    }

    private async Task ExecuteOpenPageAction(AvitoCatalogueItem item)
    {
        await driver.Navigate().GoToUrlAsync(item.UrlInfo);
    }
}
