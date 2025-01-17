using AdvSearcher.Application.Contracts.MessageListeners;

namespace AdvSearcher.Backend.TauriPlugIn.MessageListener;

public sealed class ConsoleMessageListener : IMessageListener
{
    public void Publish(string message)
    {
        Console.WriteLine(message);
    }
}
