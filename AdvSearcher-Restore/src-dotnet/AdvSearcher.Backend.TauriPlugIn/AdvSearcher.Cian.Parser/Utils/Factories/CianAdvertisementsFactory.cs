using AdvSearcher.Cian.Parser.Materials.CianComponents;
using AdvSearcher.Cian.Parser.Models.ExternalModels;
using AdvSearcher.Cian.Parser.Models.InternalModels;
using AdvSearcher.Cian.Parser.Utils.Converters;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Cian.Parser.Utils.Factories;

public sealed class CianAdvertisementsFactory
{
    private readonly CianAdvertisementCard[] _cards;
    private readonly CianDateConverter _converter;

    public CianAdvertisementsFactory(CianAdvertisementCard[] cards, CianDateConverter converter)
    {
        _cards = cards;
        _converter = converter;
    }

    public IEnumerable<IParserResponse> Construct()
    {
        List<IParserResponse> responses = new List<IParserResponse>();
        foreach (var card in _cards)
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
            responses.Add(response);
        }
        return responses;
    }

    private Result<IParsedPublisher> TryCreatePublisher(CianAdvertisementCard card) =>
        CianPublisher.Create(card.Details.MobilePhone);

    private Result<IParsedAdvertisement> TryCreateAdvertisement(CianAdvertisementCard card)
    {
        var date = _converter.Convert(card.Details.DateDescription);
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
