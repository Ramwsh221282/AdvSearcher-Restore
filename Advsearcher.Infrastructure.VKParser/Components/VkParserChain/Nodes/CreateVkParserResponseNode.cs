using AdvSearcher.Core.Tools;
using Advsearcher.Infrastructure.VKParser.Components.Converters;
using Advsearcher.Infrastructure.VKParser.Components.Requests;
using Advsearcher.Infrastructure.VKParser.Components.VkParserResponses;
using Advsearcher.Infrastructure.VKParser.Filtering;
using Advsearcher.Infrastructure.VKParser.Models.VkParsedData;
using AdvSearcher.Parser.SDK;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
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
    private readonly ParserConsoleLogger _logger;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkParserResponseNode(
        VkParserPipeLine pipeLine,
        IHttpService service,
        IHttpClient client,
        IVkParserRequestFactory factory,
        ParserConsoleLogger logger,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _converter = new VkDateConverter();
        _httpService = service;
        _httpClient = client;
        _factory = factory;
        _logger = logger;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        if (!_pipeLine.AreTokensCorrect)
        {
            _logger.Log("Vk tokens are not correct. Stopping process.");
            return;
        }
        _logger.Log("Creating VkParser Responses");
        if (_pipeLine.ItemsJson == null)
        {
            _logger.Log("Json items were null");
            return;
        }
        ParserFilter filter = new ParserFilter(_pipeLine.FilterOptions);
        foreach (var json in _pipeLine.ItemsJson.Items)
        {
            Result<IParsedAdvertisement> advertisement = CreateAdvertisement(json);
            if (!IsMatchingDateFilter(advertisement, filter))
                continue;
            Result<IParsedPublisher> publisher = await CreatePublisher(json);
            if (!IsMatchingAdvertisementFilters(advertisement, publisher, filter))
                continue;
            IParsedAttachment[] attachments = CreateAttachments(json);
            AppendInResultsCollection(advertisement, publisher, attachments);
        }
        _logger.Log($"Vk Parser Responses constructed. Count: {_pipeLine.Responses.Count}");
        if (Next != null)
        {
            _logger.Log("Executing next chain");
            await Next.ExecuteAsync();
        }
    }

    private Result<IParsedAdvertisement> CreateAdvertisement(JToken json)
    {
        _logger.Log("Creating VK Advertisement");
        if (_pipeLine.GroupInfo == null)
        {
            _logger.Log("Group info was null");
            return new Error("No group info found");
        }

        Result<IParsedAdvertisement> advertisement = VkAdvertisement.Create(
            json,
            _pipeLine.GroupInfo,
            _converter
        );
        _logger.Log($"Created Vk Advertisement");
        return advertisement;
    }

    private async Task<Result<IParsedPublisher>> CreatePublisher(JToken json)
    {
        Result<string> id = ExtractId(json);
        Result<string> postOwnerResponse = await GetPostOwnerResponse(id);
        if (id.IsFailure)
        {
            _logger.Log($"Publisher error: {id.Error}");
            return id.Error;
        }
        if (postOwnerResponse.IsFailure)
            return postOwnerResponse.Error;
        Result<IParsedPublisher> publisher = VkPublisher.Create(postOwnerResponse);
        _logger.Log($"Created Vk Publisher: {publisher.Value.Info}");
        return publisher;
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
        if (_pipeLine.Options == null)
            return new Error("No options found");
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
        _logger.Log("Adding Vk advertisement");
        _pipeLine.AddResponse(
            new VkParserResponse(advertisement.Value, attachments, publisher.Value)
        );
    }

    private bool IsMatchingDateFilter(
        Result<IParsedAdvertisement> advertisement,
        ParserFilter filter
    )
    {
        if (advertisement.IsFailure)
            return false;
        IParserFilterVisitor visitor = new VkParserDateOnlyFilterVisitor(advertisement.Value);
        return filter.IsMatchingFilters(visitor);
    }

    private bool IsMatchingAdvertisementFilters(
        Result<IParsedAdvertisement> advertisement,
        Result<IParsedPublisher> publisher,
        ParserFilter filter
    )
    {
        if (advertisement.IsFailure)
            return false;
        if (publisher.IsFailure)
            return false;
        IParserFilterVisitor visitor = new VkParserFilterVisitior(
            advertisement.Value,
            publisher.Value
        );
        return filter.IsMatchingFilters(visitor);
    }
}
