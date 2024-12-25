using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Core.Tools;
using AdvSearcher.Infrastracture.OkParser.Models.ExternalModels;
using AdvSearcher.Infrastracture.OkParser.Models.InternalModels;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;
using AdvSearcher.Infrastracture.OkParser.Utils.Materials;
using AdvSearcher.Parser.SDK.HttpParsing;
using HtmlAgilityPack;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory;

internal sealed class OkAdvertisementsFactory(
    IHttpClient client,
    IOkAdvertisementBuildersProvider builders,
    IAdvertisementDateConverter<OkParser> converter
)
{
    public async Task<Result<List<IParserResponse>>> Construct(ServiceUrl url, OkTopicNodes nodes)
    {
        List<IParserResponse> responses = new();
        foreach (var node in nodes.Nodes)
        {
            var document = new HtmlDocument();
            Result<DateOnly> date = await ConstructDate(node);
            var urlBuilder = builders.GetUrlBuilder(node, url);
            Result<string> sourceUrl = await urlBuilder.Build();
            Result<string> id = await builders.GetIdBuilder(urlBuilder).Build();
            Result<string> content = await builders
                .GetContentBuilder(urlBuilder, client, document)
                .Build();
            Result<List<string>> attachments = await builders
                .GetAttachmentsBuilder(document)
                .Build();
            Result<IParsedPublisher> publisher = await ConstructPublisher(node);
            Result<IParsedAdvertisement> advertisement = ConstructAdvertisement(
                id,
                sourceUrl,
                content,
                date
            );
            IParsedAttachment[] photos = ConstructAttachments(attachments);
            AppendSuccessfullyCreatedAdvertisement(advertisement, publisher, photos, responses);
        }
        return responses;
    }

    private void AppendSuccessfullyCreatedAdvertisement(
        Result<IParsedAdvertisement> advertisement,
        Result<IParsedPublisher> publisher,
        IParsedAttachment[] attachments,
        List<IParserResponse> responses
    )
    {
        if (advertisement.IsFailure)
            return;
        if (publisher.IsFailure)
            return;
        responses.Add(new OkParserResults(advertisement.Value, attachments, publisher.Value));
    }

    private Result<IParsedAdvertisement> ConstructAdvertisement(
        Result<string> id,
        Result<string> sourceUrl,
        Result<string> content,
        Result<DateOnly> date
    )
    {
        if (id.IsFailure)
            return id.Error;
        if (sourceUrl.IsFailure)
            return sourceUrl.Error;
        if (content.IsFailure)
            return content.Error;
        if (date.IsFailure)
            return date.Error;
        return OkParsedAdvertisement.Create(id.Value, sourceUrl.Value, content.Value, date.Value);
    }

    private IParsedAttachment[] ConstructAttachments(Result<List<string>> attachments)
    {
        List<IParsedAttachment> constructed = [];
        foreach (var attachment in attachments.Value)
        {
            Result<IParsedAttachment> result = OkParsedAttachment.Create(attachment);
            if (result.IsFailure)
                continue;
            constructed.Add(result.Value);
        }
        return constructed.ToArray();
    }

    private async Task<Result<IParsedPublisher>> ConstructPublisher(HtmlNode node)
    {
        var publisherBuilder = builders.GetPublisherBuilder(node);
        var publisherInfo = await publisherBuilder.Build();
        return OkParsedPublisher.Create(publisherInfo);
    }

    private async Task<Result<DateOnly>> ConstructDate(HtmlNode node)
    {
        var dateBuilder = builders.GetDateOnlyBuilder(node, converter);
        return await dateBuilder.Build();
    }
}
