using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Models.InternalModels;
using AdvSearcher.Infrastructure.Avito.Utils.WebDriverCommands.FetchGalleryItems;
using OpenQA.Selenium;

namespace AdvSearcher.Infrastructure.Avito;

internal sealed class AvitoAdvertisementsFactory(IWebDriver driver)
{
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public async Task Construct(
        IAdvertisementDateConverter<AvitoParserService> converter,
        List<AvitoCatalogueItem> items
    )
    {
        await ConstructInternalAdvertisements(items);
        ConstructExternalAdvertisements(items, converter);
    }

    private async Task ExecuteOpenPageAction(AvitoCatalogueItem item) =>
        await driver.Navigate().GoToUrlAsync(item.UrlInfo);

    private async Task ConstructInternalAdvertisements(List<AvitoCatalogueItem> items)
    {
        try
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
        catch { }
    }

    private void ConstructExternalAdvertisements(
        List<AvitoCatalogueItem> items,
        IAdvertisementDateConverter<AvitoParserService> converter
    )
    {
        foreach (var item in items)
        {
            Result<IParsedAdvertisement> advertisement = AvitoAdvertisement.Create(item, converter);
            if (advertisement.IsFailure)
                continue;
            Result<IParsedPublisher> publisher = AvitoPublisher.Create(item);
            if (publisher.IsFailure)
                continue;
            Result<IParsedAttachment[]> attachments = AvitoAttachment.Create(item);
            IParserResponse result = new AvitoParserResult(
                advertisement.Value,
                attachments.Value,
                publisher.Value
            );
            _results.Add(result);
        }
    }
}
