using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.Avito.Materials;
using AdvSearcher.Infrastructure.Avito.Models.InternalModels;
using AdvSearcher.Infrastructure.Avito.Utils.Converters;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Infrastructure.Avito;

internal sealed class AvitoAdvertisementsFactory
{
    public List<IParserResponse> Construct(AvitoCatalogueItem[] items, AvitoDateConverter converter)
    {
        List<IParserResponse> responses = [];
        foreach (var item in items)
        {
            Result<IParsedAdvertisement> advertisement = CreateAdvertisement(item, converter);
            Result<IParsedPublisher> publisher = CreatePublisher(item);
            IParsedAttachment[] attachments = CreateAttachments(item);
            if (advertisement.IsFailure)
                continue;
            if (publisher.IsFailure)
                continue;
            responses.Add(new AvitoParserResult(advertisement.Value, attachments, publisher.Value));
        }

        return responses;
    }

    private Result<IParsedAdvertisement> CreateAdvertisement(
        AvitoCatalogueItem item,
        AvitoDateConverter converter
    ) => AvitoAdvertisement.Create(item, converter);

    private Result<IParsedPublisher> CreatePublisher(AvitoCatalogueItem item) =>
        AvitoPublisher.Create(item);

    private IParsedAttachment[] CreateAttachments(AvitoCatalogueItem item)
    {
        Result<IParsedAttachment[]> attachments = AvitoAttachment.Create(item);
        return attachments.IsFailure ? [] : attachments.Value;
    }

    // private async Task ExecuteOpenPageAction(AvitoCatalogueItem item) =>
    //     await driver.Navigate().GoToUrlAsync(item.UrlInfo);
    //
    // private async Task ConstructInternalAdvertisements(List<AvitoCatalogueItem> items)
    // {
    //     try
    //     {
    //         foreach (var item in items)
    //         {
    //             await ExecuteOpenPageAction(item);
    //             var fetchGalleryCommand = new FetchGalleryItemsCommand(item);
    //             await fetchGalleryCommand.Execute(driver);
    //             var fetchMobilePhoneCommand = new FetchMobilePhone(item);
    //             await fetchMobilePhoneCommand.Execute(driver);
    //         }
    //     }
    //     catch { }
    // }
    //
    // private void ConstructExternalAdvertisements(
    //     List<AvitoCatalogueItem> items,
    //     IAdvertisementDateConverter<AvitoParserService> converter
    // )
    // {
    //     foreach (var item in items)
    //     {
    //         Result<IParsedAdvertisement> advertisement =
    //         if (advertisement.IsFailure)
    //             continue;
    //         Result<IParsedPublisher> publisher =
    //         if (publisher.IsFailure)
    //             continue;
    //         Result<IParsedAttachment[]> attachments =
    //         IParserResponse result = new AvitoParserResult(
    //             advertisement.Value,
    //             attachments.Value,
    //             publisher.Value
    //         );
    //         _results.Add(result);
    //     }
    // }
}
