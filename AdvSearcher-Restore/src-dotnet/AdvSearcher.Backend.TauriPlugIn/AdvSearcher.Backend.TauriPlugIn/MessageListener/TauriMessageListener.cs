using AdvSearcher.Application.Contracts.MessageListeners;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.MessageListener;

public sealed class TauriMessageListener : IMessageListener
{
    public const string NotificationsListener = "dotnet-notification";
    private readonly IEventPublisher _publisher;
    private readonly string _publishPoint;

    public TauriMessageListener(IEventPublisher publisher, string publishPoint) =>
        (_publisher, _publishPoint) = (publisher, publishPoint);

    public void Publish(string message) => _publisher.Publish(_publishPoint, message);
}
