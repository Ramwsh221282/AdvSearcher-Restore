using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.Converters;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;
using Advsearcher.Infrastructure.VKParser.Models.VkParsedData;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.HttpParsing;
using Newtonsoft.Json.Linq;

namespace Advsearcher.Infrastructure.VKParser.Components.VkParserChain.Nodes;

internal sealed class CreateVkParserResponseNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly VkDateConverter _converter;
    private readonly IHttpService _httpService;
    private readonly IHttpClient _httpClient;
    private readonly IVkParserRequestFactory _factory;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkParserResponseNode(
        VkParserPipeLine pipeLine,
        IHttpService service,
        IHttpClient client,
        IVkParserRequestFactory factory,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _converter = new VkDateConverter();
        _httpService = service;
        _httpClient = client;
        _factory = factory;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (_pipeLine.ItemsJson == null)
            return;
        foreach (var json in _pipeLine.ItemsJson.Items)
        {
            Result<IParsedAdvertisement> advertisement = CreateAdvertisement(json);
            Result<IParsedPublisher> publisher = await CreatePublisher(json);
            IParsedAttachment[] attachments = CreateAttachments(json);
            AppendInResultsCollection(advertisement, publisher, attachments);
        }
        if (Next != null)
            await Next.ExecuteAsync();
    }

    private Result<IParsedAdvertisement> CreateAdvertisement(JToken json)
    {
        if (_pipeLine.GroupInfo == null)
            return new Error("No group info found");
        return VkAdvertisement.Create(json, _pipeLine.GroupInfo, _converter);
    }

    private async Task<Result<IParsedPublisher>> CreatePublisher(JToken json)
    {
        Result<string> id = ExtractId(json);
        Result<string> postOwnerResponse = await GetPostOwnerResponse(id);
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

    private async Task<Result<string>> GetPostOwnerResponse(Result<string> id)
    {
        if (id.IsFailure)
            return id.Error;
        IHttpRequest request = _factory.CreateVkPostOwnerRequest(_pipeLine.Options, id);
        do
        {
            await _httpService.Execute(_httpClient, request);
        } while (_httpService.Result.Contains("error"));
        return _httpService.Result;
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
        _pipeLine.AddResponse(
            new VkParserResponse(advertisement.Value, attachments, publisher.Value)
        );
    }
}
