namespace AdvSearcher.Application.Contracts.MessageListeners;

public interface INotificatable
{
    public void SetNotificationPublisher(Action<string> publisher);
}
