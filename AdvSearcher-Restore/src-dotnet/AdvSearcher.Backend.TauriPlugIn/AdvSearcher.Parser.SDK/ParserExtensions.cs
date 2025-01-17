using AdvSearcher.Core.Entities.AdvertisementAttachments;
using AdvSearcher.Core.Entities.AdvertisementAttachments.ValueObjects;
using AdvSearcher.Core.Entities.Advertisements;
using AdvSearcher.Core.Entities.Advertisements.ValueObjects;
using AdvSearcher.Core.Entities.Publishers;
using AdvSearcher.Core.Entities.Publishers.ValueObjects;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;

namespace AdvSearcher.Parser.SDK;

public static class ParserExtensions
{
    public static IEnumerable<Advertisement> ToAdvertisements(
        this IEnumerable<IParserResponse> responses
    )
    {
        List<Advertisement> advertisements = [];
        foreach (var response in responses)
        {
            Result<Advertisement> advertisement = response.ToAdvertisement();
            if (advertisement.IsFailure)
                continue;
            advertisements.Add(advertisement.Value);
        }
        return advertisements;
    }

    public static Result<Advertisement> ToAdvertisement(this IParserResponse response)
    {
        Result<AdvertisementId> id = AdvertisementId.Create(ulong.Parse(response.Advertisement.Id));
        Result<AdvertisementPublishedDate> publishedDate = AdvertisementPublishedDate.Create(
            response.Advertisement.Date
        );
        Result<CreationDate> creationDate = CreationDate.Create(
            DateOnly.FromDateTime(DateTime.Now)
        );
        Result<AdvertisementContent> content = AdvertisementContent.Create(
            response.Advertisement.Content
        );
        Result<AdvertisementType> type = AdvertisementType.Create("Гостинка");
        Result<AdvertisementUrl> url = AdvertisementUrl.Create(response.Advertisement.Url);
        Result<AdvertisementServiceName> service = AdvertisementServiceName.Create(
            response.ServiceName
        );
        Result<Publisher> publisher = response.ToPublisher();
        if (id.IsFailure)
            return id.Error;
        if (publisher.IsFailure)
            return publisher.Error;
        if (publishedDate.IsFailure)
            return publishedDate.Error;
        if (creationDate.IsFailure)
            return creationDate.Error;
        if (content.IsFailure)
            return content.Error;
        if (url.IsFailure)
            return url.Error;
        if (service.IsFailure)
            return service.Error;

        Advertisement advertisement = new Advertisement(
            id,
            content,
            publishedDate,
            service,
            url,
            creationDate,
            type
        );
        List<AdvertisementAttachment> attachments = response.ToAdvertisementAttachment(
            advertisement
        );
        advertisement.SetAttachments(attachments);
        advertisement.SetPublisher(publisher);
        return advertisement;
    }

    private static List<AdvertisementAttachment> ToAdvertisementAttachment(
        this IParserResponse response,
        Advertisement advertisement
    )
    {
        List<AdvertisementAttachment> advertisementAttachments = [];
        foreach (var attachment in response.Attachments)
        {
            Result<AdvertisementAttachmentSourceUrl> url = AdvertisementAttachmentSourceUrl.Create(
                attachment.Url
            );
            if (url.IsFailure)
                continue;
            advertisementAttachments.Add(new AdvertisementAttachment(url, advertisement));
        }
        return advertisementAttachments;
    }

    private static Result<Publisher> ToPublisher(this IParserResponse response)
    {
        Result<PublisherData> data = PublisherData.Create(response.Publisher.Info);
        if (data.IsFailure)
            return data.Error;
        return new Publisher(data);
    }
}
