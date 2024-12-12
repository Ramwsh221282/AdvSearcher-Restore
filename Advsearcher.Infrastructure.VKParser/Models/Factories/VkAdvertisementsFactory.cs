using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using Advsearcher.Infrastructure.VKParser.Components;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;
using Advsearcher.Infrastructure.VKParser.Models.VkParsedData;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Models.Factories;

internal sealed class VkAdvertisementsFactory(
    RestClient client,
    VkOptions options,
    VkItemsJson json,
    VkGroupInfo info
)
{
    private readonly JToken[] _items = json.Items;
    private readonly List<IParserResponse> _data = [];
    public IReadOnlyCollection<IParserResponse> Data => _data;

    public async Task Construct(
        IVkParserRequestFactory factory,
        IAdvertisementDateConverter<VkParser> converter
    )
    {
        foreach (var item in _items)
        {
            var advertisement = VkAdvertisement.Create(item, info, converter);
            if (advertisement.IsFailure)
                continue;
            var publisher = VkPublisher.Create(item);
            if (publisher.IsFailure)
                continue;
            await publisher.Value.InitializeName(factory, client, options);
            var attachmentsCreation = VkAttachment.TryExtractAttachments(item);
            var attachments = attachmentsCreation.IsFailure ? [] : attachmentsCreation.Value;
            var response = VkParserResponse.Create(advertisement, publisher, attachments);
            _data.Add(response);
        }
    }
}
