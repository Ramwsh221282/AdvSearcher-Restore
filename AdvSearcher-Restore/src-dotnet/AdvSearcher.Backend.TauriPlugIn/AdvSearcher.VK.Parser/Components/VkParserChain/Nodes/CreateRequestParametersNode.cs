using AdvSearcher.Application.Contracts.MessageListeners;
using AdvSearcher.Core.Tools;

namespace AdvSearcher.VK.Parser.Components.VkParserChain.Nodes;

public sealed class CreateRequestParametersNode : IVkParserNode
{
    private readonly VkParserPipeLine _pipeLine;
    private readonly IMessageListener _listener;
    public VkParserPipeLine PipeLine => _pipeLine;
    public IVkParserNode? Next { get; }

    public CreateRequestParametersNode(
        VkParserPipeLine pipeLine,
        IMessageListener listener,
        IVkParserNode? next = null
    )
    {
        _pipeLine = pipeLine;
        _listener = listener;
        Next = next;
    }

    public async Task ExecuteAsync()
    {
        _listener.Publish("Получение псевдонима группы ВК.");
        _pipeLine.NotificationsPublisher?.Invoke("Получение псевдонима группы ВК.");
        _pipeLine.InstantiateOptions();
        if (!_pipeLine.AreTokensCorrect)
        {
            string message = "Токены ВК некорректны. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        if (_pipeLine.ServiceUrl == null)
        {
            string message = "Указанная ссылка ВК некорректна. Остановка процесса.";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        Result<VkRequestParameters> parameters = VkRequestParameters.Create(_pipeLine.ServiceUrl);
        if (parameters.IsFailure)
        {
            string message = $"Ошибка получения параметров. {parameters.Error.Description}";
            _pipeLine.NotificationsPublisher?.Invoke(message);
            _listener.Publish(message);
            return;
        }
        _pipeLine.SetParameters(parameters.Value);
        _listener.Publish("Параметры получены.");
        _pipeLine.NotificationsPublisher?.Invoke("Параметры получены.");
        if (Next != null)
            await Next.ExecuteAsync();
    }
}
