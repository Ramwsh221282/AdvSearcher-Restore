using AdvSearcher.Application.Contracts.MessageListeners;

namespace AdvSearcher.Backend.TauriPlugIn.MessageListener;

public sealed class CompositeMessageListener : IMessageListener
{
    private readonly List<IMessageListener> _listeners = [];

    public CompositeMessageListener AddListener(IMessageListener listener)
    {
        _listeners.Add(listener);
        return this;
    }

    public void Publish(string message) => _listeners.ForEach(l => l.Publish(message));
}
