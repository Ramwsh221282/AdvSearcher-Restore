using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastructure.CianParser.Materials.CianComponents;
using AdvSearcher.Infrastructure.CianParser.Models.ExternalModels;
using AdvSearcher.Infrastructure.CianParser.Models.InternalModels;

namespace AdvSearcher.Infrastructure.CianParser.Utils.Factories;

internal sealed class CianAdvertisementsFactory(
    List<CianAdvertisementCard> cards,
    IAdvertisementDateConverter<CianParser> converter
)
{
    private readonly List<IParserResponse> _results = [];

    public IReadOnlyCollection<IParserResponse> Results => _results;

    public void Construct()
    {
        foreach (var card in cards)
        {
            var publisher = TryCreatePublisher(card);
            if (publisher.IsFailure)
                continue;

            var advertisement = TryCreateAdvertisement(card);
            if (advertisement.IsFailure)
                continue;

            var attachments = TryCreateAttachments(card);

            IParserResponse response = new CianParserResult(
                advertisement.Value,
                attachments.ToArray(),
                publisher.Value
            );
            _results.Add(response);
        }
    }

    private Result<IParsedPublisher> TryCreatePublisher(CianAdvertisementCard card) =>
        CianPublisher.Create(card.Details.MobilePhone);

    private Result<IParsedAdvertisement> TryCreateAdvertisement(CianAdvertisementCard card)
    {
        var date = converter.Convert(card.Details.DateDescription);
        if (date.IsFailure)
            return date.Error;
        var id = card.Details.Id;
        var url = card.Details.SourceUrl;
        var content = card.Details.Description;
        return CianAdvertisement.Create(id, url, content, date);
    }

    private List<IParsedAttachment> TryCreateAttachments(CianAdvertisementCard card)
    {
        List<IParsedAttachment> attachments = [];

        foreach (var photo in card.Details.PhotoUrls)
        {
            var attachmentFactory = CianAttachment.Create(photo);
            if (attachmentFactory.IsSuccess)
                attachments.Add(attachmentFactory.Value);
        }

        return attachments;
    }
}
