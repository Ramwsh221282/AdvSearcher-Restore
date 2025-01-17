namespace AdvSearcher.Application.Contracts.Progress;

public interface IProgressable
{
    public void SetCurrentProgressValuePublisher(Action<int> actionPublisher);
    public void SetMaxProgressValuePublisher(Action<int> actionPublisher);
}
