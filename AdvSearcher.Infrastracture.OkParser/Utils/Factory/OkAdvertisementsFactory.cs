using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Entities.ServiceUrls;
using AdvSearcher.Infrastracture.OkParser.Models.ExternalModels;
using AdvSearcher.Infrastracture.OkParser.Models.InternalModels;
using AdvSearcher.Infrastracture.OkParser.Utils.Factory.Builders;
using AdvSearcher.Infrastracture.OkParser.Utils.Materials;
using AdvSearcher.Infrastracture.OkParser.Utils.OkHttpClients;
using HtmlAgilityPack;

namespace AdvSearcher.Infrastracture.OkParser.Utils.Factory;

internal sealed class OkAdvertisementsFactory(ServiceUrl url, OkTopicNodes nodes)
{
    private readonly List<IParserResponse> _results = [];
    public IReadOnlyCollection<IParserResponse> Results => _results;

    public async Task Construct(
        IOkAdvertisementBuildersProvider builders,
        IAdvertisementDateConverter<OkParser> converter,
        IOkHttpClientProvider provider
    )
    {
        using var client = provider.Provide();
        foreach (var node in nodes.Nodes)
        {
            var publisherBuilder = builders.GetPublisherBuilder(node);
            var publisher = await publisherBuilder.Build();
            if (publisher.IsFailure)
                continue;

            var dateBuilder = builders.GetDateOnlyBuilder(node, converter);
            var date = await dateBuilder.Build();
            if (date.IsFailure)
                continue;

            var urlBuilder = builders.GetUrlBuilder(node, url);
            var url1 = await urlBuilder.Build();
            if (url1.IsFailure)
                continue;

            var idBuilder = builders.GetIdBuilder(urlBuilder);
            var id = await idBuilder.Build();
            if (id.IsFailure)
                continue;

            var document = new HtmlDocument();
            var contentBuilder = builders.GetContentBuilder(urlBuilder, client, document);
            var content = await contentBuilder.Build();
            if (content.IsFailure)
                continue;

            var attachmentsBuilder = builders.GetAttachmentsBuilder(document);
            var attachments = await attachmentsBuilder.Build();

            var parsedAdvertisement = OkParsedAdvertisement.Create(id, url1, content, date);
            if (parsedAdvertisement.IsFailure)
                continue;

            var parsedPublisher = OkParsedPublisher.Create(publisher);
            if (parsedPublisher.IsFailure)
                continue;

            var parsedAttachments = new List<OkParsedAttachment>();
            foreach (var attachment in attachments.Value)
            {
                var result = OkParsedAttachment.Create(attachment);
                if (result.IsFailure)
                    continue;
                parsedAttachments.Add(result);
            }

            IParserResponse response = OkParserResults.Create(
                parsedAdvertisement,
                parsedAttachments,
                parsedPublisher
            );
            _results.Add(response);
        }
    }
}
