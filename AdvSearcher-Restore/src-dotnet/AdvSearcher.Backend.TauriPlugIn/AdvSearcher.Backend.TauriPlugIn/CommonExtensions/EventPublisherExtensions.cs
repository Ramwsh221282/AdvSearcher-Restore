using AdvSearcher.Core.Tools;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.CommonExtensions;

public static class EventPublisherExtensions
{
    public static void PublishError(this IEventPublisher publisher, Error error, string listener) =>
        publisher.Publish(listener, error.Description);
}
