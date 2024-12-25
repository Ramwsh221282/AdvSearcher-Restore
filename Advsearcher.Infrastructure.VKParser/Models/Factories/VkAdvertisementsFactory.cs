using AdvSearcher.Application.Abstractions.Parsers;
using AdvSearcher.Core.Entities.Advertisements.Abstractions;
using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.Responses;
using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;
using Advsearcher.Infrastructure.VKParser.Models.VkParsedData;
using AdvSearcher.Parser.SDK.HttpParsing;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Advsearcher.Infrastructure.VKParser.Models.Factories;

internal sealed class VkAdvertisementsFactory(
    IHttpService service,
    IHttpClient client,
    IAdvertisementDateConverter<VkParser> converter,
    IVkParserRequestFactory factory
)
{
    private readonly List<IParserResponse> _data = [];

    public async Task<Result<List<IParserResponse>>> Construct(
        VkOptions options,
        Result<VkItemsJson> json,
        Result<VkGroupInfo> info
    )
    {
        if (json.IsFailure)
            return json.Error;

        foreach (var item in json.Value.Items)
        {
            Result<IParsedAdvertisement> advertisement = CreateAdvertisement(item, info);
            Result<IParsedPublisher> publisher = await CreatePublisher(item, options);
            Result<IParsedAttachment[]> attachments = CreateAttachments(item);
            AppendInResultsCollection(advertisement, publisher, attachments);
        }
        return _data;
    }

    private Result<IParsedAdvertisement> CreateAdvertisement(JToken item, Result<VkGroupInfo> info)
    {
        if (info.IsFailure)
            return info.Error;
        return VkAdvertisement.Create(item, info.Value, converter);
    }

    private async Task<Result<IParsedPublisher>> CreatePublisher(JToken item, VkOptions options)
    {
        Result<string> id = ExtractId(item);
        Result<string> postOwnerResponse = await GetPostOwnerResponse(id, options);
        if (id.IsFailure)
            return id.Error;
        if (postOwnerResponse.IsFailure)
            return postOwnerResponse.Error;
        return VkPublisher.Create(id, postOwnerResponse);
    }

    private Result<string> ExtractId(JToken json)
    {
        JToken? fromIdToken = json["from_id"];
        if (fromIdToken == null)
            return new Error("Не удалось получить ID автора");
        return fromIdToken.ToString();
    }

    private async Task<Result<string>> GetPostOwnerResponse(Result<string> id, VkOptions options)
    {
        if (id.IsFailure)
            return id.Error;
        IHttpRequest request = factory.CreateVkPostOwnerRequest(options, id);
        do
        {
            await service.Execute(client, request);
        } while (service.Result.Contains("error"));
        return service.Result;
    }

    private IParsedAttachment[] CreateAttachments(JToken json)
    {
        Result<IParsedAttachment[]> attachments = VkAttachment.TryExtractAttachments(json);
        return attachments.IsFailure ? [] : attachments.Value;
    }

    private void AppendInResultsCollection(
        Result<IParsedAdvertisement> advertisement,
        Result<IParsedPublisher> publisher,
        IParsedAttachment[] attachments
    )
    {
        if (advertisement.IsFailure)
            return;
        if (publisher.IsFailure)
            return;
        _data.Add(new VkParserResponse(advertisement.Value, attachments, publisher.Value));
    }
}
