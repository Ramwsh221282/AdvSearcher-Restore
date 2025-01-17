using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.VK.Parser.Components.Requests;
using AdvSearcher.VK.Parser.Components.Responses;

namespace AdvSearcher.VK.Parser.Components.VkParserChain.Nodes;

public sealed class CreateVkItemsJsonNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly IHttpClient _httpClient;
    private readonly IHttpService _httpService;
    private readonly IVkParserRequestFactory _factory;
    private readonly IMessageListener _listener;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkItemsJsonNode(
        VkParserPipeLine pipeLine,
        IHttpClient httpClient,
        IHttpService service,
        IVkParserRequestFactory factory,
        IMessageListener listener,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = httpClient;
        _httpService = service;
        _factory = factory;
        _listener = listener;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Запрос на получения постов ВК.");
        _pipeLine.NotificationsPublisher?.Invoke("Запрос на получения постов ВК.");
        if (!_pipeLine.AreTokensCorrect)
        {
            string message = "Токены ВК некорректны. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        _listener.Publish("Проверка информации о группе.");
        if (_pipeLine.GroupInfo == null)
        {
            string message = "Информация о группе некорректна. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        if (_pipeLine.Options == null)
        {
            string message = "Настройки ВК некорректны. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        _listener.Publish("Отправка запроса на получение постов ВК.");
        _pipeLine.NotificationsPublisher?.Invoke("Отправка запроса на получение постов ВК.");
        IHttpRequest request = _factory.CreateWallPostRequest(
            _pipeLine.Options,
            _pipeLine.GroupInfo
        );
        VkItemsJsonFactory jsonFactory = new VkItemsJsonFactory(_httpService, request, _httpClient);
        Result<VkItemsJson> jsons = await jsonFactory.CreateVkItemsJson();
        if (jsons.IsFailure)
        {
            string message = "Ответ на получение постов ВК был с ошибкой.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        _pipeLine.SetItemsJson(jsons.Value);
        _listener.Publish("Посты ВК проинициализированы.");
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
