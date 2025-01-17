namespace AdvSearcher.Application.Contracts.MessageListeners;

public interface IListenable
{
    void SetMessageListener(IMessageListener listener);
}
