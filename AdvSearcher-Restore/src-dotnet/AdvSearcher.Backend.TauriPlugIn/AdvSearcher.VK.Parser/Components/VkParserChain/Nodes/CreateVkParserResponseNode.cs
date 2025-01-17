using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.Contracts;
using AdvSearcher.Parser.SDK.Filtering;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.VK.Parser.Components.Converters;
using AdvSearcher.VK.Parser.Components.Requests;
using AdvSearcher.VK.Parser.Components.VkParserResponses;
using AdvSearcher.VK.Parser.Filtering;
using AdvSearcher.VK.Parser.Models;
using Newtonsoft.Json.Linq;

namespace AdvSearcher.VK.Parser.Components.VkParserChain.Nodes;

public sealed class CreateVkParserResponseNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly VkDateConverter _converter;
    private readonly IHttpService _httpService;
    private readonly IHttpClient _httpClient;
    private readonly IVkParserRequestFactory _factory;
    private readonly IMessageListener _listener;
    private int _currentProgress;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkParserResponseNode(
        VkParserPipeLine pipeLine,
        IHttpService service,
        IHttpClient client,
        IVkParserRequestFactory factory,
        IMessageListener listener,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _converter = new VkDateConverter();
        _httpService = service;
        _httpClient = client;
        _factory = factory;
        _listener = listener;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Создание результатов парсинга ВК.");
        _pipeLine.NotificationsPublisher?.Invoke("Создание результатов парсинга ВК.");
        if (!_pipeLine.AreTokensCorrect)
        {
            string message = "Токены ВК некорректны. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        if (_pipeLine.ItemsJson == null)
        {
            string message = "Посты ВК не были получены. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        ParserFilter filter = new ParserFilter(_pipeLine.FilterOptions);
        _pipeLine.PublishMaxProgressValue(_pipeLine.ItemsJson.Items.Count());
        foreach (var json in _pipeLine.ItemsJson.Items)
        {
            Result<IParsedAdvertisement> advertisement = CreateAdvertisement(json);
            if (!IsMatchingDateFilter(advertisement, filter))
            {
                _currentProgress++;
                _pipeLine.PublishCurrentProgressValue(_currentProgress);
                continue;
            }
            Result<IParsedPublisher> publisher = await CreatePublisher(json);
            if (!IsMatchingAdvertisementFilters(advertisement, publisher, filter))
            {
                _currentProgress++;
                _pipeLine.PublishCurrentProgressValue(_currentProgress);
                continue;
            }
            IParsedAttachment[] attachments = CreateAttachments(json);
            AppendInResultsCollection(advertisement, publisher, attachments);
            _currentProgress++;
            _pipeLine.PublishCurrentProgressValue(_currentProgress);
        }
        _pipeLine.NotificationsPublisher?.Invoke(
            $"Получено {_pipeLine.Responses.Count} результатов."
        );
        _listener.Publish($"Получено {_pipeLine.Responses.Count} результатов.");
        _httpClient.Dispose();
        if (Next != null)
            await Next.ExecuteAsync();
    }

    private Result<IParsedAdvertisement> CreateAdvertisement(JToken json)
    {
        if (_pipeLine.GroupInfo == null)
            return new Error("No group info found");
        Result<IParsedAdvertisement> advertisement = VkAdvertisement.Create(
            json,
            _pipeLine.GroupInfo,
            _converter
        );
        return advertisement;
    }

    private async Task<Result<IParsedPublisher>> CreatePublisher(JToken json)
    {
        Result<string> id = ExtractId(json);
        Result<string> postOwnerResponse = await GetPostOwnerResponse(id);
        if (id.IsFailure)
            return id.Error;
        if (postOwnerResponse.IsFailure)
            return postOwnerResponse.Error;
        Result<IParsedPublisher> publisher = VkPublisher.Create(postOwnerResponse);
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
