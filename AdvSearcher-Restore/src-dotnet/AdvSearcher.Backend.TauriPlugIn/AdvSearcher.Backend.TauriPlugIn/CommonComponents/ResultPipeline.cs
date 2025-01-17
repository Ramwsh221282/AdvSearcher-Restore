using AdvSearcher.Backend.TauriPlugIn.CommonExtensions;
using AdvSearcher.Core.Tools;
using TauriDotNetBridge.Contracts;

namespace AdvSearcher.Backend.TauriPlugIn.CommonComponents;

public sealed class ResultPipeline(IEventPublisher publisher, string listener)
{
    private readonly IEventPublisher _publisher = publisher;
    private readonly string _listener = listener;
    private readonly List<Error> _errors = [];

    public TValue Do<TValue>(Func<Result<TValue>> func)
    {
        Result<TValue> result = func();
        if (result.IsFailure)
            _errors.Add(result.Error);

        if (_errors.Any())
        {
            Error error = _errors[^1];
            _publisher.PublishError(error, _listener);
            return Result<TValue>.Failure(error);
        }

        return result;
    }
}
