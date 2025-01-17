using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Tools;
using AdvSearcher.Parser.SDK.HttpParsing;
using AdvSearcher.VK.Parser.Components.Requests;
using AdvSearcher.VK.Parser.Components.Responses;

namespace AdvSearcher.VK.Parser.Components.VkParserChain.Nodes;

public sealed class CreateVkGroupInfoNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly IHttpService _httpService;
    private readonly IHttpClient _httpClient;
    private readonly IMessageListener _listener;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateVkGroupInfoNode(
        VkParserPipeLine pipeLine,
        IHttpService httpService,
        IHttpClient httpClient,
        IMessageListener listener,
        IVkParserNode? node = null
    )
    {
        _pipeLine = pipeLine;
        _httpClient = httpClient;
        _httpService = httpService;
        _listener = listener;
        Next = node;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Запрос на получение информации о группе ВК");
        _pipeLine.NotificationsPublisher?.Invoke("Запрос на получение информации о группе ВК");
        try
        {
            _listener.Publish("Проверка токенов ВК.");
            _pipeLine.NotificationsPublisher?.Invoke("Проверка токенов ВК.");
            if (!_pipeLine.AreTokensCorrect)
            {
                string message = "Токены ВК некорректны. Остановка процесса.";
                _pipeLine.NotificationsPublisher?.Invoke(message);
                _listener.Publish(message);
                return;
            }
            _listener.Publish("Считывание псевдонима группы.");
            _pipeLine.NotificationsPublisher?.Invoke("Считывание псевдонима группы.");
            if (_pipeLine.Parameters == null)
            {
                string message = "Псевдоним группы некорректен. Остановка процесса.";
                _pipeLine.NotificationsPublisher?.Invoke(message);
                _listener.Publish(message);
                return;
            }
            _listener.Publish("Считывание настроек ВК.");
            _pipeLine.NotificationsPublisher?.Invoke("Считывание настроек ВК.");
            if (_pipeLine.Options == null)
            {
                string message = "Настройки ВК некорректны";
                _pipeLine.NotificationsPublisher?.Invoke(message);
                _listener.Publish(message);
                return;
            }
            _listener.Publish("Создание запроса");
            _pipeLine.NotificationsPublisher?.Invoke("Создание запроса");
            IHttpRequest request = new VkGroupOwnerIdRequest(
                _pipeLine.Options,
                _pipeLine.Parameters
            );
            VKGroupInfoResponseFactory factory = new VKGroupInfoResponseFactory(
                request,
                _httpClient,
                _httpService
            );
            Result<VkGroupInfo> info = await factory.CreateGroupInfo(_pipeLine.Parameters);
            _listener.Publish("Запрос выполнен.");
            _pipeLine.NotificationsPublisher?.Invoke("Запрос выполнен.");
            if (info.IsFailure)
            {
                _listener.Publish(
                    $"Ошибка получения информации о группе. {info.Error.Description}"
                );
                _pipeLine.NotificationsPublisher?.Invoke(
                    $"Ошибка получения информации о группе. {info.Error.Description}"
                );
                return;
            }
            _pipeLine.SetGroupInfo(info.Value);
            _listener.Publish("Информация о группе получена.");
            _pipeLine.NotificationsPublisher?.Invoke("Информация о группе получена.");
            if (Next != null)
                await Next.ExecuteAsync();
        }
        catch (Exception ex)
        {
            _listener.Publish($"Произошло исключение: {ex.Message}");
            _pipeLine.NotificationsPublisher?.Invoke($"Произошло исключение: {ex.Message}");
        }
    }
}
