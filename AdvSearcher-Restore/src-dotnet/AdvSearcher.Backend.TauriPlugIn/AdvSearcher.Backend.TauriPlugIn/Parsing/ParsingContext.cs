namespace AdvSearcher.Backend.TauriPlugIn.Parsing;

public sealed class ParsingContext
{
    private readonly IParsingStrategy _strategy;
    private readonly Action<int> _currentProgress;
    private readonly Action<int> _maxProgress;
    private readonly Action<string> _notificationsPublisher;

    public ParsingContext(
        IParsingStrategy strategy,
        Action<int> currentProgress,
        Action<int> maxProgress,
        Action<string> notificationsPublisher
    )
    {
        _strategy = strategy;
        _currentProgress = currentProgress;
        _maxProgress = maxProgress;
        _notificationsPublisher = notificationsPublisher;
    }

    public async Task ProcessParsing() =>
        await _strategy.Perform(_currentProgress, _maxProgress, _notificationsPublisher);
}
