namespace AdvSearcher.Application.Contracts.MessageListeners;

public interface IMessageListener
{
    void Publish(string message);
}
