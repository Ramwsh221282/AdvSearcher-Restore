namespace AdvSearcher.Backend.TauriPlugIn.Parsing;

public interface IParsingStrategy
{
    Task Perform(
        Action<int> currentProgress,
        Action<int> maxProgress,
        Action<string> notificationsPublisher
    );
}
